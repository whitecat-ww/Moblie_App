using System;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Networking;

namespace SosTrackerApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // 页面加载时自动获取位置和网络
            this.Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, EventArgs e)
        {
            await GetLocationAndConnectivity();
        }

        private async Task GetLocationAndConnectivity()
        {
            try
            {
                // 1. Get Network Status
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                bool isConnected = accessType == NetworkAccess.Internet;
                StatusLabel.Text = isConnected ? "Online" : "Offline";
                StatusLabel.TextColor = isConnected ? Colors.Green : Colors.Red;

                // 2. Get Geolocation
                // Try to get last known location first (faster)
                var location = await Geolocation.Default.GetLastKnownLocationAsync();

                // If null, request real-time location
                if (location == null)
                {
                    location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                }

                if (location != null)
                {
                    LatLabel.Text = location.Latitude.ToString("F5");
                    LongLabel.Text = location.Longitude.ToString("F5");
                }
                else
                {
                    LatLabel.Text = "Unknown";
                    LongLabel.Text = "Unknown";
                }
            }
            catch (Exception ex)
            {
                // Fallback for emulator issues
                LatLabel.Text = "Error";
                await DisplayAlert("Location Error", "Please ensure GPS is set in Emulator Extended Controls.", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string inputId = TripIdEntry.Text;

            // --- Validation Logic (Q2 Requirement) ---
            if (string.IsNullOrWhiteSpace(inputId))
            {
                await DisplayAlert("Validation Error", "Trip ID cannot be empty.", "OK");
                return;
            }
            if (inputId.Length < 3)
            {
                await DisplayAlert("Validation Error", "Trip ID must be at least 3 characters.", "OK");
                return;
            }

            // --- Save to Database ---
            try
            {
                var db = new DatabaseService();
                var log = new TripLog
                {
                    TripId = inputId,
                    GeoLocation = $"{LatLabel.Text}, {LongLabel.Text}",
                    CreatedAt = DateTime.Now
                };

                await db.AddLogAsync(log);
                await DisplayAlert("Success", "Trip log saved securely to local database.", "OK");

                // Clear input
                TripIdEntry.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Database Error", ex.Message, "OK");
            }
        }
    }
}
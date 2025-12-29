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
                // 获取网络
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                bool isConnected = accessType == NetworkAccess.Internet;
                StatusLabel.Text = isConnected ? "在线 (Online)" : "离线 (Offline)";
                StatusLabel.TextColor = isConnected ? Colors.Green : Colors.Red;

                // 获取位置
                var location = await Geolocation.Default.GetLastKnownLocationAsync();
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
            }
            catch (Exception ex)
            {
                // 模拟器请确保开启位置权限
                LatLabel.Text = "Error";
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string inputId = TripIdEntry.Text;

            // 验证逻辑
            if (string.IsNullOrWhiteSpace(inputId))
            {
                await DisplayAlert("错误", "ID 不能为空", "OK");
                return;
            }
            if (inputId.Length < 3)
            {
                await DisplayAlert("错误", "ID 长度需大于3位", "OK");
                return;
            }

            // 保存逻辑
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
                await DisplayAlert("成功", "已保存", "OK");
                TripIdEntry.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("失败", ex.Message, "OK");
            }
        }
    }
}
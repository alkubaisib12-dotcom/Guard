using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace GuardPrototype
{
    public partial class SetupPage : ContentPage
    {
        private readonly HttpClient _httpClient = new();
        private const string BaseUrl = "https://localhost:7169"; // Replace with your API base

        public SetupPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Convert numeric inputs
                int.TryParse(TotalSmartPlugs.Text, out int totalSmartPlugs);
                int.TryParse(TotalDevices.Text, out int totalDevices);
                int.TryParse(TotalDevicesBlaster.Text, out int totalDevicesBlaster);

                // Build payload that matches SetupConfigDto
                var setupPayload = new
                {
                    Rooms = new[]
                    {
                        new {
                            RoomName = RoomName.Text?.Trim(),
                            TotalSmartPlugs = totalSmartPlugs,
                            TotalDevices = totalDevices
                        }
                    },
                    SmartPlugs = string.IsNullOrWhiteSpace(SmartPlugDeviceId.Text) &&
                                 string.IsNullOrWhiteSpace(SmartPlugRegion.Text)
                        ? new object[] { } // send empty list if no plug info
                        : new[] {
                            new {
                                DeviceId = SmartPlugDeviceId.Text?.Trim(),
                                Region = SmartPlugRegion.Text?.Trim()
                            }
                        },
                    Irblasters = new[]
                    {
                        new {
                            TotalDevicesBlaster = totalDevicesBlaster,
                            Devices = string.IsNullOrWhiteSpace(Devices.Text)
                                ? new List<string>()
                                : new List<string>(Devices.Text.Split(',', StringSplitOptions.RemoveEmptyEntries)),
                            LocationInTheRoom = LocationInTheRoom.Text?.Trim()
                        }
                    },
                    Cameras = new[]
                    {
                        new {
                            CameraUsername = Camera_username.Text?.Trim(),
                            CameraPassword = Cameras_Password.Text?.Trim(),
                            CameraIpaddress = Cameras_IPaddress.Text?.Trim()
                        }
                    },
                    Sensors = new[]
                    {
                        new {
                            AirSensorApikey = AirSensor_APIKey.Text?.Trim(),
                            ShellyPmminiDeviceIp = ShellyPMMini_DeviceIP.Text?.Trim(),
                            ShellyFloodDeviceIp = Shellyflood_DeviceIP.Text?.Trim()
                        }
                    }
                };

                // Serialize and send
                var json = JsonSerializer.Serialize(setupPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{BaseUrl}/api/setup", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Setup saved successfully!", "OK");
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Failed to save: {errorText}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "OK");
            }
        }
    }
}

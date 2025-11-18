using GuardPrototype.IRessentials;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuardPrototype
{
    public partial class IRSetupPage : ContentPage
    {
        private readonly AddDevices.LocalServer _localServer = new();
        private List<IRManager.RM3Device> _discoveredDevices = new();

        public IRSetupPage()
        {
            InitializeComponent();
            _ = _localServer.StartAsync(); // Fire and forget
            OnDiscoverClicked(this, EventArgs.Empty); // Auto-discover on load
        }

        private async void OnDiscoverClicked(object sender, EventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = "discover_rm3.py",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();

                _discoveredDevices = JsonSerializer.Deserialize<List<IRManager.RM3Device>>(output) ?? new();
                DeviceListView.ItemsSource = _discoveredDevices;

                StatusLabel.Text = $"Discovered {_discoveredDevices.Count} RM3 devices.";
                StatusLabel.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Discovery failed: {ex.Message}";
                StatusLabel.TextColor = Colors.Red;
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var appliance = new AddDevices.Appliance
            {
                Name = DeviceNameEntry.Text?.Trim(),
                Type = DeviceTypeEntry.Text?.Trim(),
                RM3Ip = RM3IpEntry.Text?.Trim(),
                CommandHex = CommandHexEntry.Text?.Trim()
            };

            if (string.IsNullOrWhiteSpace(appliance.Name) ||
                string.IsNullOrWhiteSpace(appliance.Type) ||
                string.IsNullOrWhiteSpace(appliance.RM3Ip) ||
                string.IsNullOrWhiteSpace(appliance.CommandHex))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            _localServer.RegisterAppliance(appliance);
            bool success = await SendCommandToDevice(appliance.RM3Ip, appliance.CommandHex);

            StatusLabel.Text = success
                ? $"Appliance '{appliance.Name}' registered and command sent!"
                : $"Appliance '{appliance.Name}' registered, but command failed.";
            StatusLabel.TextColor = success ? Colors.Green : Colors.Red;

            int totalRooms = Preferences.Get("TotalRooms", 1);
            int currentIndex = Preferences.Get("CurrentRoomIndex", 1);

            if (currentIndex <= totalRooms)
            {
                await Navigation.PushAsync(new SetupPage());
            }
            else
            {
                string openAiEndpoint = "https://api.openai.com/v1/chat/completions";
                string openAiKey = "your_openai_api_key"; // Replace with actual key
                int userId = Preferences.Get("UserId", 0);

                for (int i = 0; i < RoomSetupManager.Rooms.Count; i++)
                {
                    var config = RoomSetupManager.Rooms[i];
                    var roomService = new RoomService(config, userId, i + 1, openAiEndpoint, openAiKey);
                    roomService.Start();
                    await roomService.MonitorCameraAsync();
                }

                await DisplayAlert("Setup Complete", "All rooms are configured and services are running.", "OK");
                await Navigation.PushAsync(new MainPage());
            }
        }

        private async Task<bool> SendCommandToDevice(string ip, string hex)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = $"discover_rm3.py {ip} {hex}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();

                Debug.WriteLine($"IR command output: {output}");
                return output.Contains("Command sent");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"IR command error: {ex.Message}");
                return false;
            }
        }
    }
}

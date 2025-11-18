using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;

namespace GuardPrototype
{
    public partial class CreateAccountPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7169";

        public CreateAccountPage()
        {
            InitializeComponent();

            // TODO: Remove this in production and use valid SSL certificates
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();
            bool parsed = int.TryParse(TotalRoomsEntry.Text, out int totalRooms);

            // Validate inputs before sending
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || !parsed || totalRooms < 1 || totalRooms > 50)
            {
                await DisplayAlert("Error", "Fill all fields correctly. TotalRooms must be between 1 and 50.", "OK");
                return;
            }

            // Optional: Show a loading indicator (add an ActivityIndicator to XAML if desired)
            // LoadingIndicator.IsVisible = true;

            // Manually serialize using PascalCase property names (matches your API DTO)
            var payload = new
            {
                Username = username,
                Password = password,
                TotalRooms = totalRooms
            };

            string json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Ensures "Username", not "username"
            });

            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{BaseUrl}/api/users/register", content);
                var responseText = await response.Content.ReadAsStringAsync();

                // LoadingIndicator.IsVisible = false;

                if (response.IsSuccessStatusCode)
                {
                    // Success: Store preferences and navigate
                    Preferences.Set("TotalRooms", totalRooms);
                    Preferences.Set("CurrentRoomIndex", 1);
                    await DisplayAlert("Success", "Account created successfully!", "OK");
                    await Navigation.PushAsync(new SetupPage());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Handle 400 errors (e.g., duplicate username from API)
                    await DisplayAlert("Error", $"Registration failed: {responseText}", "OK");
                }
                else
                {
                    // Handle other errors (e.g., 500, 404)
                    await DisplayAlert("Error", $"Status {(int)response.StatusCode}: {response.StatusCode}\n{responseText}", "OK");
                }
            }
            catch (Exception ex)
            {
                // LoadingIndicator.IsVisible = false;
                await DisplayAlert("Exception", $"Network error: {ex.Message}", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
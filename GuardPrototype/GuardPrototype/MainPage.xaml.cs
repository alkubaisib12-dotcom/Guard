using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace GuardPrototype
{
    public partial class MainPage : ContentPage
    {
        private const string ApiBaseUrl = "https://localhost:7169";
        
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            var loginPayload = new
            {
                username = username,
                password = password
            };

            try
            {
                using var client = new HttpClient();
                var json = JsonSerializer.Serialize(loginPayload);
                var content = new StringContent(
    JsonSerializer.Serialize(loginPayload, new JsonSerializerOptions { PropertyNamingPolicy = null }),
    Encoding.UTF8,
    "application/json");


                var response = await client.PostAsync($"{ApiBaseUrl}/api/users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginResponse>(responseBody);

                    Preferences.Set("auth_token", loginResult.Token);
                    Preferences.Set("user_id", loginResult.UserId);

                    await DisplayAlert("Success", "Login successful!", "OK");
                    await Navigation.PushAsync(new SetupPage());
                }
                else
                {
                    await DisplayAlert("Error", "Invalid credentials.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountPage());
        }

        private class LoginResponse
        {
            public string Token { get; set; }
            public int UserId { get; set; }
        }
    }
}


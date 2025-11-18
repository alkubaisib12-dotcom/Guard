using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace GuardPrototype
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://localhost:7169"; // adjust if needed

        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        // Step 1: Verify username ? reveal Rooms step
        private async void OnUsernameVerified(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter your username.", "OK");
                return;
            }

            // Show Rooms step
            RoomsLabel.IsVisible = true;
            RoomsEntry.IsVisible = true;
            VerifyRoomsButton.IsVisible = true;
        }

        // Step 2: Verify rooms count ? reveal Password step
        private async void OnVerifyRoomsClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string roomsText = RoomsEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(roomsText))
            {
                await DisplayAlert("Error", "Please enter both username and total rooms.", "OK");
                return;
            }

            if (!int.TryParse(roomsText, out int claimedRooms))
            {
                await DisplayAlert("Error", "Total rooms must be a number.", "OK");
                return;
            }

            var payload = new { username, roomCount = claimedRooms };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/api/users/verify-room-count", content);
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Room count does not match. Try again.", "OK");
                return;
            }

            await DisplayAlert("Success", "Verification successful! You can now reset your password.", "OK");

            // Show Password step
            PasswordLabel.IsVisible = true;
            NewPasswordEntry.IsVisible = true;
            ConfirmPasswordEntry.IsVisible = true;
            ChangePasswordButton.IsVisible = true;
        }

        // Step 3: Reset password
        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            string newPass = NewPasswordEntry.Text?.Trim();
            string confirmPass = ConfirmPasswordEntry.Text?.Trim();
            string username = UsernameEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                await DisplayAlert("Error", "Please fill in both password fields.", "OK");
                return;
            }

            if (newPass != confirmPass)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var payload = new { username, newPassword = newPass, confirmPassword = confirmPass };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/api/users/reset-password", content);
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "Failed to reset password.", "OK");
                return;
            }

            await DisplayAlert("Success", "Your password has been changed successfully!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

using System;
using Microsoft.Maui.Controls;

namespace GuardPrototype
{
    public partial class DeviceSetupGuidePage : ContentPage
    {
        public DeviceSetupGuidePage()
        {
            InitializeComponent();
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            try
            {
                // Navigate back if possible
                if (Navigation?.NavigationStack?.Count > 1)
                {
                    await Navigation.PopAsync();
                }
                else
                {
#if WINDOWS || MACCATALYST
                    Application.Current.Quit(); // Desktop platforms
#else
                    // On mobile, just pop to root
                    await Navigation.PopToRootAsync();
#endif
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to exit: {ex.Message}", "OK");
            }
        }
    }
}

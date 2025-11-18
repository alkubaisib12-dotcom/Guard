using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace GuardPrototype
{
    public partial class RecommendationsPage : ContentPage
    {
        private readonly RoomService _room;

        public RecommendationsPage(RoomService room)
        {
            InitializeComponent();
            _room = room;
            LoadRecommendations();
        }

        private T? GetPrivateClassField<T>(object obj, string fieldName) where T : class
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(obj) as T;
        }

        private T GetPrivateStructField<T>(object obj, string fieldName) where T : struct
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var value = field.GetValue(obj);
                if (value is T typedValue)
                    return typedValue;
            }
            return default;
        }

        private async void LoadRecommendations()
        {
            var camera = GetPrivateClassField<CameraMonitorService>(_room, "_cameraMonitor");
            var model = GetPrivateClassField<OpenAIRecommendations>(_room, "_recommendationsModel");
            var actions = GetPrivateClassField<OpenAIActionsModel>(_room, "_actionsModel");
            var fetcher = GetPrivateClassField<DeviceFetcher>(_room, "_deviceFetcher");
            var roomId = GetPrivateStructField<int>(_room, "_roomId");

            if (camera == null || model == null || actions == null || fetcher == null)
            {
                await DisplayAlert("Error", "One or more services are not initialized.", "OK");
                return;
            }

            var observation = await camera.GetFreshObservationAsync();
            var devices = await fetcher.GetDevicesAsync(roomId);
            var action = await actions.DecideActionAsync(observation, devices);
            var recommendation = await model.GetRecommendationAsync(observation, action);

            RecommendationsLayout.Children.Clear();

            // Futuristic styled labels
            RecommendationsLayout.Children.Add(new Label
            {
                Text = $"Hazard: {observation}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#0A84FF") // Neon blue
            });

            RecommendationsLayout.Children.Add(new Label
            {
                Text = $"Suggested Action: {action}",
                FontSize = 18,
                TextColor = Colors.White
            });

            RecommendationsLayout.Children.Add(new Label
            {
                Text = $"Recommendation: {recommendation}",
                FontSize = 18,
                TextColor = Colors.White
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace GuardPrototype
{
    public partial class DashboardPage : ContentPage
    {
        private readonly List<RoomService> _roomServices;

        public DashboardPage(List<RoomService> roomServices)
        {
            InitializeComponent();
            _roomServices = roomServices;
            LoadAllRooms();
        }

        private async void LoadAllRooms()
        {
            foreach (var room in _roomServices)
            {
                // Futuristic card container
                var layout = new Frame
                {
                    Padding = 15,
                    CornerRadius = 15,
                    BackgroundColor = Color.FromArgb("#1A1A1A"), // Dark futuristic panel
                    HasShadow = true,
                    Margin = new Thickness(0, 0, 0, 20)
                };

                var stack = new VerticalStackLayout
                {
                    Spacing = 10
                };

                // Room title
                stack.Children.Add(new Label
                {
                    Text = room.RoomName,
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0A84FF") // Neon blue
                });

                // Camera snapshot if available
                var camera = GetPrivateField<CameraMonitorService>(room, "_cameraMonitor");
                if (camera != null)
                {
                    var imageBytes = await camera.GetSnapshotAsync();
                    if (imageBytes?.Length > 0)
                    {
                        stack.Children.Add(new Image
                        {
                            Source = ImageSource.FromStream(() => new MemoryStream(imageBytes)),
                            HeightRequest = 200,
                            Aspect = Aspect.AspectFill
                        });
                    }
                }

                // Recommendations button
                var recsButton = new Button
                {
                    Text = "View Recommendations",
                    BackgroundColor = Color.FromArgb("#0A84FF"),
                    TextColor = Colors.White,
                    CornerRadius = 20,
                    HeightRequest = 45,
                    FontAttributes = FontAttributes.Bold,
                    Shadow = new Shadow
                    {
                        Brush = Brush.Black,
                        Offset = new Point(2, 2),
                        Radius = 8,
                        Opacity = 0.6f
                    }
                };

                recsButton.Clicked += async (s, e) =>
                {
                    await Navigation.PushAsync(new RecommendationsPage(room));
                };

                stack.Children.Add(recsButton);
                layout.Content = stack;

                MainLayout.Children.Add(layout);
            }
        }

        private T? GetPrivateField<T>(object obj, string fieldName) where T : class
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(obj) as T;
        }
    }
}

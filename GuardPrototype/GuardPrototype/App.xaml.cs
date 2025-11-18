using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.Generic;

namespace GuardPrototype
{
    public partial class App : Application
    {
        private readonly List<RoomService> _activeRooms = new();

        public App()
        {
            InitializeComponent();

            string token = Preferences.Get("auth_token", "");
            int totalRooms = Preferences.Get("TotalRooms", 0);
            int currentRoom = Preferences.Get("CurrentRoomIndex", 1);

            if (string.IsNullOrWhiteSpace(token))
            {
                MainPage = new NavigationPage(new MainPage());
                return;
            }

            if (currentRoom <= totalRooms)
            {
                MainPage = new NavigationPage(new SetupPage());
                return;
            }

            string openAiEndpoint = "https://api.openai.com/v1/chat/completions";
            string openAiKey = "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";
            int userId = Preferences.Get("user_id", 0);

            foreach (var config in RoomSetupManager.Rooms)
            {
                var roomService = new RoomService(config, userId, _activeRooms.Count + 1, openAiEndpoint, openAiKey);
                roomService.Start();
                _activeRooms.Add(roomService);
            }

            MainPage = new NavigationPage(new DashboardPage(_activeRooms));
        }
    }
}

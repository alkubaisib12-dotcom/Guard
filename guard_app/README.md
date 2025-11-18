# Guard - AI-Powered Home Safety System

A comprehensive Flutter application for proactive home safety monitoring with AI-powered hazard detection and prevention.

## 🚀 Key Features

### ✅ Complete Authentication System
- **Login/Signup** - Full authentication flow with local storage
- **Onboarding** - Beautiful 4-screen introduction for first-time users
- **Session Management** - Persistent login state across app restarts
- **Secure Logout** - Confirmation dialog with data cleanup

### 🏠 Core Functionality
- **Dashboard** - Real-time overview with risk charts and quick actions
- **Live Cameras** - AI-powered hazard detection with visual alerts
- **Sensors** - Multi-sensor monitoring (air quality, water leak, current, motion)
- **Insights** - Analytics, trends, and AI-powered predictions

### 👤 Profile Management
- **Editable Profile** - Update name, email, location, home type
- **System Health** - Real-time status of sensors and devices
- **Account Info** - Member since, risk level, subscription status
- **Statistics** - Sensors, rooms, alerts, and prevented accidents

### ⚙️ Settings & Preferences
- **Persistent Settings** - All preferences saved to local storage
- **Notifications** - Hazard alerts, auto-actions, child safety mode
- **System Configuration** - Sensor calibration, device management
- **Automations** - Configure smart rules and automated responses

## 📱 App Screens (15 Total)

### Authentication Flow
1. **Onboarding Page** - 4-screen feature introduction (first launch only)
2. **Login Page** - Email/password authentication
3. **Signup Page** - New account creation

### Main App (4 Bottom Nav Tabs)
4. **Dashboard** - Home overview with stats and risk charts
5. **Cameras** - Grid view of all cameras
6. **Sensors** - List of all sensors with status
7. **Insights** - Analytics and AI predictions

### Additional Pages
8. **Live Hazard Page** - Detailed camera view with AI detection
9. **Hazard Predictions** - AI-predicted risks and actions
10. **House Map** - Room-by-room safety status
11. **Activity Log** - Timeline of all events
12. **Recommendations** - Safety improvement suggestions
13. **Settings** - App preferences and configuration
14. **Profile** - User account and stats
15. **Edit Profile** - Modify user information
16. **Automations** - Smart rules management

## 🎨 Design

- **Theme**: Futuristic dark theme with neon blue (#0A84FF)
- **Gradients**: Black to blue gradients throughout
- **Cards**: Rounded corners (15px) with shadows
- **Icons**: Material Design with color-coded status
- **Responsive**: Adapts to all screen sizes

## 💾 Data Persistence

Using **SharedPreferences** for local storage:
- User credentials (name, email)
- Authentication state
- App settings (notifications, modes)
- User preferences (home type, location)

## 🔧 Technical Stack

### Dependencies
```yaml
dependencies:
  flutter: sdk
  shared_preferences: ^2.2.2  # Local storage
  intl: ^0.18.1               # Date formatting
  cupertino_icons: ^1.0.2      # iOS-style icons
```

### Architecture
- **Services**: StorageService for data persistence
- **Models**: Sensor, Camera, HazardPrediction, Room, ActivityLog, Automation
- **State Management**: StatefulWidgets with setState
- **Navigation**: MaterialPageRoute with Navigator

## 🚀 Getting Started

### Prerequisites
- Flutter SDK (>=3.0.0)
- Dart SDK

### Installation

1. **Clone and navigate:**
```bash
cd guard_app
```

2. **Install dependencies:**
```bash
flutter pub get
```

3. **Run the app:**

**For Web:**
```bash
flutter run -d web-server
# Or for Chrome
flutter run -d chrome
```

**For Mobile (Android/iOS):**
```bash
flutter run
```

**For Desktop:**
```bash
flutter run -d windows  # or macos / linux
```

## 🎯 App Flow

### First Launch
1. **Onboarding** (4 screens) → Shows key features
2. **Login/Signup** → User authentication
3. **Main App** → Dashboard with full functionality

### Subsequent Launches
- **If logged in** → Directly to Dashboard
- **If not logged in** → Login page

## 🔐 Demo Credentials

**Login accepts any email/password for demo purposes**

Example:
- Email: `demo@guard.com`
- Password: `anything`

## 📊 Dummy Data Included

All features work with realistic dummy data:
- 8 sensors (air quality, water leak, current, motion, etc.)
- 5 cameras with locations and statuses
- 6 AI hazard predictions with confidence scores
- 7 rooms with safety statuses
- 10 activity log entries
- 7 automation rules
- Weekly trends and statistics

## 🎬 Use Cases Demonstrated

### Baby Safety
- **Edge of couch** → Camera detects, alerts parents
- **Front door** → Motion triggers alarm
- **Balcony/stairs** → Sirens and SMS alerts

### Fire Prevention
- **Candle near curtain** → Smart actuator moves curtain
- **Air quality drops** → Detects smoke, alerts user

### Electrical Safety
- **Overloaded power strip** → Auto shut-off via smart plug
- **Current spike** → Device disconnected automatically

### Water Damage
- **Hidden leaks** → Under-sink sensors detect moisture
- **Washing machine** → Water valve closes automatically

## 📱 Supported Platforms

- ✅ Web (Chrome, Firefox, Safari, Edge)
- ✅ Android
- ✅ iOS
- ✅ Windows
- ✅ macOS
- ✅ Linux

## 🔄 Recent Updates

### Version 1.0.0
- ✅ Complete authentication system
- ✅ Beautiful onboarding flow
- ✅ Editable profile with persistence
- ✅ Settings persistence
- ✅ Logout functionality
- ✅ Local storage integration
- ✅ 15 fully functional screens

## 📂 Project Structure

```
guard_app/
├── lib/
│   ├── main.dart                    # App entry & auth flow
│   ├── services/
│   │   └── storage_service.dart     # Local storage
│   ├── models/                      # Data models
│   │   ├── sensor.dart
│   │   ├── camera.dart
│   │   ├── hazard_prediction.dart
│   │   ├── room.dart
│   │   ├── activity_log.dart
│   │   └── automation.dart
│   └── pages/                       # UI screens
│       ├── onboarding_page.dart
│       ├── login_page.dart
│       ├── signup_page.dart
│       ├── dashboard_page.dart
│       ├── cameras_page.dart
│       ├── live_hazard_page.dart
│       ├── sensors_page.dart
│       ├── hazard_predictions_page.dart
│       ├── house_map_page.dart
│       ├── activity_log_page.dart
│       ├── recommendations_page.dart
│       ├── settings_page.dart
│       ├── profile_page.dart
│       ├── edit_profile_page.dart
│       ├── automations_page.dart
│       └── insights_page.dart
├── web/                             # Web platform files
│   ├── index.html
│   ├── manifest.json
│   └── icons/
└── pubspec.yaml
```

## 🔮 Future Enhancements

- [ ] Connect to real backend API (GuardDBcallsAPI)
- [ ] Integrate actual sensors (Shelly, IQAir, etc.)
- [ ] WebSocket for real-time updates
- [ ] Camera streaming
- [ ] Push notifications
- [ ] Multi-language support
- [ ] Dark/light theme toggle
- [ ] Export safety reports

## 📄 License

All rights reserved.

## 🆘 Support

For issues or questions, please contact the development team or refer to the Flutter documentation.

---

**Built with Flutter** | **Powered by AI** | **Designed for Safety**

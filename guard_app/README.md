# Guard - AI-Powered Home Safety System

A comprehensive Flutter mobile application for home safety monitoring with AI-powered hazard detection.

## Features

### Core Functionality
- **Dashboard**: Real-time overview of home safety status
- **Live Cameras**: Monitor IP cameras with AI hazard detection
- **Sensors**: Track air quality, water leaks, current monitoring, and more
- **Insights**: AI-powered analytics and safety trends

### Pages Included

1. **Dashboard Page** - Main overview with statistics and quick actions
2. **Cameras Page** - Grid view of all cameras with live status
3. **Live Hazard Page** - Detailed camera view with AI threat detection
4. **Sensors Overview** - List of all sensors with current readings
5. **Hazard Predictions** - AI-predicted hazards and actions taken
6. **House Map** - Room-by-room status overview
7. **Activity Log** - Timeline of all system events
8. **Recommendations** - Safety improvement suggestions
9. **Settings** - App configuration and preferences
10. **Profile** - User account and system health
11. **Automations** - Smart rules and automated actions
12. **Insights** - Analytics, trends, and AI analysis

## Design

- **Theme**: Futuristic dark theme with neon blue accents (#0A84FF)
- **Style**: Modern, clean UI with gradient backgrounds
- **Navigation**: Bottom navigation bar with 4 main tabs

## Data Models

- `Sensor` - Sensor information and status
- `Camera` - Camera details and location
- `HazardPrediction` - AI-detected hazards
- `Room` - Room status and safety level
- `ActivityLog` - System event logs
- `Automation` - Smart automation rules

## Getting Started

### Prerequisites
- Flutter SDK (>=3.0.0)
- Dart SDK

### Installation

1. Navigate to the guard_app directory:
```bash
cd guard_app
```

2. Install dependencies:
```bash
flutter pub get
```

3. Run the app:
```bash
flutter run
```

## Current Status

This is a **prototype with dummy data**. All sensors, cameras, and hazards are simulated for demonstration purposes.

## Next Steps

- Connect to real backend API (GuardDBcallsAPI)
- Integrate with actual sensors (Shelly devices, IQAir, etc.)
- Add WebSocket for real-time updates
- Implement user authentication
- Add camera streaming

## Project Structure

```
guard_app/
├── lib/
│   ├── main.dart              # App entry point
│   ├── models/                # Data models
│   │   ├── sensor.dart
│   │   ├── camera.dart
│   │   ├── hazard_prediction.dart
│   │   ├── room.dart
│   │   ├── activity_log.dart
│   │   └── automation.dart
│   └── pages/                 # UI pages
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
│       ├── automations_page.dart
│       └── insights_page.dart
└── pubspec.yaml
```

## License

All rights reserved.

import 'package:flutter/material.dart';
import '../services/storage_service.dart';
import 'automations_page.dart';
import 'login_page.dart';
import 'profile_page.dart';

class SettingsPage extends StatefulWidget {
  const SettingsPage({super.key});

  @override
  State<SettingsPage> createState() => _SettingsPageState();
}

class _SettingsPageState extends State<SettingsPage> {
  bool hazardNotifications = true;
  bool autoActions = true;
  bool childSafetyMode = true;
  bool nightMode = false;

  @override
  void initState() {
    super.initState();
    _loadSettings();
  }

  void _loadSettings() {
    setState(() {
      hazardNotifications = StorageService.getBool('hazardNotifications', defaultValue: true);
      autoActions = StorageService.getBool('autoActions', defaultValue: true);
      childSafetyMode = StorageService.getBool('childSafetyMode', defaultValue: true);
      nightMode = StorageService.getBool('nightMode', defaultValue: false);
    });
  }

  Future<void> _logout() async {
    final confirm = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: const Color(0xFF1A1A1A),
        title: const Text(
          'Logout',
          style: TextStyle(color: Colors.white),
        ),
        content: const Text(
          'Are you sure you want to logout?',
          style: TextStyle(color: Colors.white70),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context, false),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () => Navigator.pop(context, true),
            style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
            child: const Text('Logout'),
          ),
        ],
      ),
    );

    if (confirm == true && mounted) {
      await StorageService.logout();
      if (mounted) {
        Navigator.of(context).pushAndRemoveUntil(
          MaterialPageRoute(builder: (context) => const LoginPage()),
          (route) => false,
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Settings',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
      ),
      body: ListView(
        padding: const EdgeInsets.all(15),
        children: [
          // Notifications Section
          const Text(
            'Notifications',
            style: TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
          const SizedBox(height: 10),
          Card(
            child: Column(
              children: [
                SwitchListTile(
                  title: const Text(
                    'Hazard Notifications',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Get alerts for detected hazards',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  value: hazardNotifications,
                  activeColor: const Color(0xFF0A84FF),
                  onChanged: (value) {
                    setState(() {
                      hazardNotifications = value;
                    });
                    StorageService.setBool('hazardNotifications', value);
                  },
                ),
                const Divider(height: 1),
                SwitchListTile(
                  title: const Text(
                    'Auto-Actions',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Allow AI to control smart devices',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  value: autoActions,
                  activeColor: const Color(0xFF0A84FF),
                  onChanged: (value) {
                    setState(() {
                      autoActions = value;
                    });
                    StorageService.setBool('autoActions', value);
                  },
                ),
                const Divider(height: 1),
                SwitchListTile(
                  title: const Text(
                    'Child Safety Mode',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Enhanced monitoring for children',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  value: childSafetyMode,
                  activeColor: const Color(0xFF0A84FF),
                  onChanged: (value) {
                    setState(() {
                      childSafetyMode = value;
                    });
                    StorageService.setBool('childSafetyMode', value);
                  },
                ),
                const Divider(height: 1),
                SwitchListTile(
                  title: const Text(
                    'Night Mode Monitoring',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Increased sensitivity during nighttime',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  value: nightMode,
                  activeColor: const Color(0xFF0A84FF),
                  onChanged: (value) {
                    setState(() {
                      nightMode = value;
                    });
                    StorageService.setBool('nightMode', value);
                  },
                ),
              ],
            ),
          ),

          const SizedBox(height: 30),

          // System Section
          const Text(
            'System',
            style: TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
          const SizedBox(height: 10),
          Card(
            child: Column(
              children: [
                ListTile(
                  leading: const Icon(Icons.sensors, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Sensor Calibration',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Adjust sensor sensitivity',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    _showInfoDialog(
                      context,
                      'Sensor Calibration',
                      'Here you can adjust the sensitivity levels of your sensors:\n\n• Motion sensors: Detection range\n• Air quality: Threshold levels\n• Water leak: Moisture sensitivity\n• Current sensors: Overload limits',
                    );
                  },
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.devices, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Manage Devices',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Add or remove smart devices',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    _showInfoDialog(
                      context,
                      'Manage Devices',
                      'Connected Devices:\n\n• 5 IP Cameras\n• 3 Smart Plugs\n• 2 Shelly Sensors\n• 1 IR Blaster\n\nTap "+ Add Device" to connect new smart home devices to Guard.',
                    );
                  },
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.auto_awesome, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Automations',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Configure smart rules',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => const AutomationsPage(),
                      ),
                    );
                  },
                ),
              ],
            ),
          ),

          const SizedBox(height: 30),

          // Account Section
          const Text(
            'Account',
            style: TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
          const SizedBox(height: 10),
          Card(
            child: Column(
              children: [
                ListTile(
                  leading: const Icon(Icons.person, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Profile',
                    style: TextStyle(color: Colors.white),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    Navigator.pop(context);
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const ProfilePage()),
                    );
                  },
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.privacy_tip, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Privacy & Security',
                    style: TextStyle(color: Colors.white),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    _showInfoDialog(
                      context,
                      'Privacy & Security',
                      'Your privacy is important:\n\n✓ Camera feeds are encrypted\n✓ Data stored locally\n✓ No third-party sharing\n✓ Two-factor authentication available\n✓ Activity logs protected\n\nAll sensor data is processed on-device using AI.',
                    );
                  },
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.help, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Help & Support',
                    style: TextStyle(color: Colors.white),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    _showInfoDialog(
                      context,
                      'Help & Support',
                      'Need help?\n\n📧 Email: support@guard.ai\n📞 Phone: +971-4-123-4567\n💬 Live Chat: Available 24/7\n📚 FAQ: guard.ai/help\n\nOur team is here to assist you with:\n• Setup & installation\n• Troubleshooting\n• Device configuration\n• Billing questions',
                    );
                  },
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.info, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'About',
                    style: TextStyle(color: Colors.white),
                  ),
                  subtitle: Text(
                    'Version 1.0.0',
                    style: TextStyle(color: Colors.grey[400], fontSize: 12),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {
                    _showInfoDialog(
                      context,
                      'About Guard',
                      'Guard v1.0.0\n\nAI-Powered Home Safety System\n\n© 2024 Guard Technologies\nAll rights reserved.\n\nGuard uses advanced AI and computer vision to predict and prevent home hazards before they occur.\n\nProactive protection for modern homes.',
                    );
                  },
                ),
              ],
            ),
          ),

          const SizedBox(height: 30),

          // Logout Button
          SizedBox(
            width: double.infinity,
            child: ElevatedButton.icon(
              onPressed: _logout,
              icon: const Icon(Icons.logout),
              label: const Text('Logout'),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.red,
                foregroundColor: Colors.white,
                padding: const EdgeInsets.all(15),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _showInfoDialog(BuildContext context, String title, String message) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: const Color(0xFF1A1A1A),
        title: Text(
          title,
          style: const TextStyle(color: Color(0xFF0A84FF)),
        ),
        content: Text(
          message,
          style: const TextStyle(color: Colors.white),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('OK'),
          ),
        ],
      ),
    );
  }
}

import 'package:flutter/material.dart';
import 'automations_page.dart';

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
                  onTap: () {},
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
                  onTap: () {},
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
                  onTap: () {},
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.privacy_tip, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Privacy & Security',
                    style: TextStyle(color: Colors.white),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {},
                ),
                const Divider(height: 1),
                ListTile(
                  leading: const Icon(Icons.help, color: Color(0xFF0A84FF)),
                  title: const Text(
                    'Help & Support',
                    style: TextStyle(color: Colors.white),
                  ),
                  trailing: Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
                  onTap: () {},
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
                  onTap: () {},
                ),
              ],
            ),
          ),

          const SizedBox(height: 30),

          // Logout Button
          SizedBox(
            width: double.infinity,
            child: ElevatedButton.icon(
              onPressed: () {},
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
}

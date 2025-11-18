import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../services/storage_service.dart';
import 'edit_profile_page.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  String _userName = '';
  String _userEmail = '';
  String _homeType = '';
  String _location = '';

  @override
  void initState() {
    super.initState();
    _loadProfile();
  }

  void _loadProfile() {
    setState(() {
      _userName = StorageService.getUserName();
      _userEmail = StorageService.getUserEmail();
      _homeType = StorageService.getString('homeType', defaultValue: 'Apartment');
      _location = StorageService.getString('location', defaultValue: 'Dubai, UAE');
    });
  }

  Future<void> _navigateToEdit() async {
    final result = await Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => const EditProfilePage()),
    );

    if (result == true) {
      _loadProfile();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Profile',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.edit, color: Color(0xFF0A84FF)),
            onPressed: _navigateToEdit,
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            // Profile Header
            CircleAvatar(
              radius: 60,
              backgroundColor: const Color(0xFF0A84FF).withOpacity(0.2),
              child: const Icon(
                Icons.person,
                size: 60,
                color: Color(0xFF0A84FF),
              ),
            ),
            const SizedBox(height: 20),
            Text(
              _userName,
              style: const TextStyle(
                fontSize: 28,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 5),
            Text(
              _userEmail,
              style: TextStyle(
                fontSize: 14,
                color: Colors.grey[400],
              ),
            ),
            const SizedBox(height: 30),

            // Stats Cards
            Row(
              children: [
                Expanded(
                  child: _buildStatCard('8', 'Sensors', Icons.sensors),
                ),
                const SizedBox(width: 15),
                Expanded(
                  child: _buildStatCard('5', 'Rooms', Icons.home),
                ),
              ],
            ),
            const SizedBox(height: 15),
            Row(
              children: [
                Expanded(
                  child: _buildStatCard('12', 'Alerts', Icons.warning),
                ),
                const SizedBox(width: 15),
                Expanded(
                  child: _buildStatCard('4', 'Prevented', Icons.shield),
                ),
              ],
            ),

            const SizedBox(height: 30),

            // Information Card
            Card(
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text(
                      'Account Information',
                      style: TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                    ),
                    const SizedBox(height: 20),
                    _buildInfoRow(Icons.home_work, 'Home Type', _homeType),
                    const Divider(height: 30),
                    _buildInfoRow(Icons.location_on, 'Location', _location),
                    const Divider(height: 30),
                    _buildInfoRow(
                      Icons.calendar_today,
                      'Member Since',
                      DateFormat('MMMM yyyy').format(DateTime.now()),
                    ),
                    const Divider(height: 30),
                    _buildInfoRow(Icons.security, 'Risk Level', 'Low', color: Colors.green),
                  ],
                ),
              ),
            ),

            const SizedBox(height: 20),

            // System Health Card
            Card(
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        const Icon(
                          Icons.health_and_safety,
                          color: Color(0xFF0A84FF),
                          size: 24,
                        ),
                        const SizedBox(width: 10),
                        const Text(
                          'System Health',
                          style: TextStyle(
                            fontSize: 18,
                            fontWeight: FontWeight.bold,
                            color: Colors.white,
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 20),
                    _buildHealthRow('Sensors Online', '8/8', 1.0, Colors.green),
                    const SizedBox(height: 15),
                    _buildHealthRow('Cameras Active', '5/5', 1.0, Colors.green),
                    const SizedBox(height: 15),
                    _buildHealthRow('Smart Devices', '12/12', 1.0, Colors.green),
                    const SizedBox(height: 20),
                    Text(
                      'Last system check: Today, ${DateFormat('HH:mm').format(DateTime.now())}',
                      style: TextStyle(
                        fontSize: 12,
                        color: Colors.grey[400],
                      ),
                    ),
                  ],
                ),
              ),
            ),

            const SizedBox(height: 20),

            // Subscription Card
            Card(
              color: const Color(0xFF0A84FF).withOpacity(0.1),
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  children: [
                    Row(
                      children: [
                        Icon(
                          Icons.stars,
                          color: Colors.amber[300],
                          size: 28,
                        ),
                        const SizedBox(width: 15),
                        const Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                'Premium Plan',
                                style: TextStyle(
                                  fontSize: 18,
                                  fontWeight: FontWeight.bold,
                                  color: Colors.white,
                                ),
                              ),
                              SizedBox(height: 5),
                              Text(
                                'Active until Dec 31, 2024',
                                style: TextStyle(
                                  fontSize: 12,
                                  color: Colors.white70,
                                ),
                              ),
                            ],
                          ),
                        ),
                        const Icon(
                          Icons.check_circle,
                          color: Colors.green,
                          size: 24,
                        ),
                      ],
                    ),
                    const SizedBox(height: 15),
                    SizedBox(
                      width: double.infinity,
                      child: OutlinedButton(
                        onPressed: () {
                          showDialog(
                            context: context,
                            builder: (context) => AlertDialog(
                              backgroundColor: const Color(0xFF1A1A1A),
                              title: const Text(
                                'Manage Subscription',
                                style: TextStyle(color: Color(0xFF0A84FF)),
                              ),
                              content: const Text(
                                'Premium Plan - Active\n\n✓ Unlimited sensors\n✓ AI hazard detection\n✓ Real-time alerts\n✓ Cloud storage (50GB)\n✓ Priority support\n\nNext billing: Dec 31, 2024\n\$19.99/month',
                                style: TextStyle(color: Colors.white),
                              ),
                              actions: [
                                TextButton(
                                  onPressed: () => Navigator.pop(context),
                                  child: const Text('Close'),
                                ),
                                ElevatedButton(
                                  onPressed: () {
                                    Navigator.pop(context);
                                    ScaffoldMessenger.of(context).showSnackBar(
                                      const SnackBar(
                                        content: Text('Subscription management coming soon'),
                                        backgroundColor: Color(0xFF0A84FF),
                                      ),
                                    );
                                  },
                                  style: ElevatedButton.styleFrom(
                                    backgroundColor: const Color(0xFF0A84FF),
                                  ),
                                  child: const Text('Upgrade'),
                                ),
                              ],
                            ),
                          );
                        },
                        style: OutlinedButton.styleFrom(
                          foregroundColor: const Color(0xFF0A84FF),
                          side: const BorderSide(color: Color(0xFF0A84FF)),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(8),
                          ),
                        ),
                        child: const Text('Manage Subscription'),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatCard(String value, String label, IconData icon) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            Icon(icon, color: const Color(0xFF0A84FF), size: 32),
            const SizedBox(height: 10),
            Text(
              value,
              style: const TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 5),
            Text(
              label,
              style: TextStyle(
                fontSize: 12,
                color: Colors.grey[400],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildInfoRow(IconData icon, String label, String value, {Color? color}) {
    return Row(
      children: [
        Icon(icon, size: 20, color: color ?? const Color(0xFF0A84FF)),
        const SizedBox(width: 15),
        Expanded(
          child: Text(
            label,
            style: TextStyle(
              fontSize: 14,
              color: Colors.grey[400],
            ),
          ),
        ),
        Text(
          value,
          style: TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.w600,
            color: color ?? Colors.white,
          ),
        ),
      ],
    );
  }

  Widget _buildHealthRow(String label, String value, double progress, Color color) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              label,
              style: const TextStyle(
                fontSize: 14,
                color: Colors.white,
              ),
            ),
            Text(
              value,
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: color,
              ),
            ),
          ],
        ),
        const SizedBox(height: 8),
        LinearProgressIndicator(
          value: progress,
          backgroundColor: Colors.grey[800],
          valueColor: AlwaysStoppedAnimation<Color>(color),
        ),
      ],
    );
  }
}

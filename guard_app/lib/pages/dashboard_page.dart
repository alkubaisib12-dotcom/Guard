import 'package:flutter/material.dart';
import 'cameras_page.dart';
import 'sensors_page.dart';
import 'activity_log_page.dart';

class DashboardPage extends StatelessWidget {
  const DashboardPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Guard',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 28,
            color: Color(0xFF0A84FF),
          ),
        ),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.fromLTRB(20, 20, 20, 100),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Summary Cards
            GridView.count(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              crossAxisCount: 2,
              crossAxisSpacing: 15,
              mainAxisSpacing: 15,
              childAspectRatio: 1.3,
              children: [
                _buildStatCard(
                  'Risks Detected',
                  '12',
                  'This week',
                  Icons.warning_amber_rounded,
                  Colors.orange,
                ),
                _buildStatCard(
                  'Accidents Prevented',
                  '4',
                  'All time',
                  Icons.shield,
                  Colors.green,
                ),
                _buildStatCard(
                  'Active Sensors',
                  '8',
                  'Online',
                  Icons.sensors,
                  const Color(0xFF0A84FF),
                ),
                _buildStatCard(
                  'Rooms Monitored',
                  '5',
                  'Protected',
                  Icons.home,
                  Colors.purple,
                ),
              ],
            ),

            const SizedBox(height: 30),

            // Risk Level Chart
            const Text(
              'Risk Level (Last 7 Days)',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            _buildRiskChart(),

            const SizedBox(height: 30),

            // Current Status
            const Text(
              'Current Status',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            _buildStatusCard(),

            const SizedBox(height: 30),

            // Quick Links
            const Text(
              'Quick Actions',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            _buildQuickLinks(context),
          ],
        ),
      ),
    );
  }

  Widget _buildStatCard(
    String title,
    String value,
    String subtitle,
    IconData icon,
    Color color,
  ) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(15),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Icon(icon, color: color, size: 28),
                Text(
                  value,
                  style: TextStyle(
                    fontSize: 32,
                    fontWeight: FontWeight.bold,
                    color: color,
                  ),
                ),
              ],
            ),
            Flexible(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: const TextStyle(
                      fontSize: 13,
                      fontWeight: FontWeight.w600,
                      color: Colors.white,
                    ),
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                  Text(
                    subtitle,
                    style: TextStyle(
                      fontSize: 11,
                      color: Colors.grey[400],
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildRiskChart() {
    final riskData = [1, 0, 2, 3, 1, 4, 1];
    final maxRisk = 5;
    final days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

    return Card(
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              crossAxisAlignment: CrossAxisAlignment.end,
              children: List.generate(7, (index) {
                return Column(
                  children: [
                    Container(
                      width: 30,
                      height: 100 * (riskData[index] / maxRisk),
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          begin: Alignment.bottomCenter,
                          end: Alignment.topCenter,
                          colors: [
                            const Color(0xFF0A84FF),
                            const Color(0xFF0A84FF).withOpacity(0.5),
                          ],
                        ),
                        borderRadius: BorderRadius.circular(8),
                      ),
                    ),
                    const SizedBox(height: 8),
                    Text(
                      days[index],
                      style: TextStyle(
                        fontSize: 10,
                        color: Colors.grey[400],
                      ),
                    ),
                  ],
                );
              }),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildStatusCard() {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          children: [
            Row(
              children: [
                Container(
                  width: 12,
                  height: 12,
                  decoration: const BoxDecoration(
                    color: Colors.green,
                    shape: BoxShape.circle,
                  ),
                ),
                const SizedBox(width: 10),
                const Text(
                  'Overall Risk: Low',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 15),
            _buildStatusRow(Icons.security, 'Monitoring', 'ON', Colors.green),
            const SizedBox(height: 10),
            _buildStatusRow(Icons.access_time, 'Last Scan', '23 seconds ago', const Color(0xFF0A84FF)),
          ],
        ),
      ),
    );
  }

  Widget _buildStatusRow(IconData icon, String label, String value, Color color) {
    return Row(
      children: [
        Icon(icon, color: color, size: 20),
        const SizedBox(width: 10),
        Text(
          '$label: ',
          style: TextStyle(
            fontSize: 14,
            color: Colors.grey[400],
          ),
        ),
        Text(
          value,
          style: TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.w600,
            color: color,
          ),
        ),
      ],
    );
  }

  Widget _buildQuickLinks(BuildContext context) {
    return Column(
      children: [
        _buildQuickLinkButton(
          context,
          'View Live Cameras',
          Icons.videocam,
          Colors.red,
          () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const CamerasPage()),
            );
          },
        ),
        const SizedBox(height: 10),
        _buildQuickLinkButton(
          context,
          'View Sensors',
          Icons.sensors,
          const Color(0xFF0A84FF),
          () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const SensorsPage()),
            );
          },
        ),
        const SizedBox(height: 10),
        _buildQuickLinkButton(
          context,
          'View Activity Log',
          Icons.list_alt,
          Colors.orange,
          () {
            Navigator.push(
              context,
              MaterialPageRoute(builder: (context) => const ActivityLogPage()),
            );
          },
        ),
      ],
    );
  }

  Widget _buildQuickLinkButton(
    BuildContext context,
    String text,
    IconData icon,
    Color color,
    VoidCallback onTap,
  ) {
    return InkWell(
      onTap: onTap,
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(20),
          child: Row(
            children: [
              Icon(icon, color: color, size: 28),
              const SizedBox(width: 15),
              Expanded(
                child: Text(
                  text,
                  style: const TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.w600,
                    color: Colors.white,
                  ),
                ),
              ),
              Icon(Icons.arrow_forward_ios, color: Colors.grey[600], size: 18),
            ],
          ),
        ),
      ),
    );
  }
}

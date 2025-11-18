import 'package:flutter/material.dart';
import 'hazard_predictions_page.dart';
import 'house_map_page.dart';
import 'recommendations_page.dart';

class InsightsPage extends StatelessWidget {
  const InsightsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Insights',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // AI Summary Card
            Card(
              color: const Color(0xFF0A84FF).withOpacity(0.1),
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: [
                        Container(
                          padding: const EdgeInsets.all(10),
                          decoration: BoxDecoration(
                            color: const Color(0xFF0A84FF).withOpacity(0.2),
                            borderRadius: BorderRadius.circular(12),
                          ),
                          child: const Icon(
                            Icons.psychology,
                            color: Color(0xFF0A84FF),
                            size: 28,
                          ),
                        ),
                        const SizedBox(width: 15),
                        const Expanded(
                          child: Text(
                            'AI-Powered Analysis',
                            style: TextStyle(
                              fontSize: 20,
                              fontWeight: FontWeight.bold,
                              color: Colors.white,
                            ),
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 15),
                    const Text(
                      'Your home safety score has improved by 23% this week. The AI has prevented 4 potential accidents and detected 12 safety concerns.',
                      style: TextStyle(
                        fontSize: 14,
                        color: Colors.white70,
                        height: 1.5,
                      ),
                    ),
                  ],
                ),
              ),
            ),

            const SizedBox(height: 30),

            // Quick Access Grid
            const Text(
              'Quick Access',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            GridView.count(
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              crossAxisCount: 2,
              crossAxisSpacing: 15,
              mainAxisSpacing: 15,
              childAspectRatio: 1.1,
              children: [
                _buildQuickAccessCard(
                  context,
                  'Hazard Predictions',
                  Icons.warning_amber_rounded,
                  Colors.orange,
                  '6 Active',
                  () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => const HazardPredictionsPage(),
                      ),
                    );
                  },
                ),
                _buildQuickAccessCard(
                  context,
                  'House Map',
                  Icons.map,
                  Colors.purple,
                  '7 Rooms',
                  () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => const HouseMapPage(),
                      ),
                    );
                  },
                ),
                _buildQuickAccessCard(
                  context,
                  'Recommendations',
                  Icons.lightbulb,
                  Colors.amber,
                  '6 New',
                  () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => const RecommendationsPage(),
                      ),
                    );
                  },
                ),
                _buildQuickAccessCard(
                  context,
                  'Safety Score',
                  Icons.shield,
                  Colors.green,
                  '87/100',
                  () {},
                ),
              ],
            ),

            const SizedBox(height: 30),

            // Weekly Trends
            const Text(
              'Weekly Trends',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            _buildTrendCard(
              'Hazard Detection Rate',
              '+15%',
              'Compared to last week',
              Icons.trending_up,
              Colors.green,
            ),
            const SizedBox(height: 10),
            _buildTrendCard(
              'False Alarms',
              '-8%',
              'AI accuracy improving',
              Icons.trending_down,
              Colors.green,
            ),
            const SizedBox(height: 10),
            _buildTrendCard(
              'Response Time',
              '2.3s',
              'Average alert response',
              Icons.speed,
              const Color(0xFF0A84FF),
            ),

            const SizedBox(height: 30),

            // Most Active Sensors
            const Text(
              'Most Active Sensors',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            Card(
              child: Padding(
                padding: const EdgeInsets.all(15),
                child: Column(
                  children: [
                    _buildSensorActivityRow('Air Quality - Kitchen', 45, Colors.orange),
                    const SizedBox(height: 15),
                    _buildSensorActivityRow('Motion - Hallway', 32, const Color(0xFF0A84FF)),
                    const SizedBox(height: 15),
                    _buildSensorActivityRow('Camera - Balcony', 28, Colors.red),
                    const SizedBox(height: 15),
                    _buildSensorActivityRow('Smart Plug - Workstation', 19, Colors.purple),
                  ],
                ),
              ),
            ),

            const SizedBox(height: 30),

            // Time-based Analysis
            const Text(
              'Peak Activity Hours',
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 15),
            Card(
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  children: [
                    _buildTimeSlotRow('Morning (6AM-12PM)', '34%', 0.34),
                    const SizedBox(height: 15),
                    _buildTimeSlotRow('Afternoon (12PM-6PM)', '45%', 0.45),
                    const SizedBox(height: 15),
                    _buildTimeSlotRow('Evening (6PM-12AM)', '18%', 0.18),
                    const SizedBox(height: 15),
                    _buildTimeSlotRow('Night (12AM-6AM)', '3%', 0.03),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildQuickAccessCard(
    BuildContext context,
    String title,
    IconData icon,
    Color color,
    String subtitle,
    VoidCallback onTap,
  ) {
    return InkWell(
      onTap: onTap,
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(15),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                padding: const EdgeInsets.all(12),
                decoration: BoxDecoration(
                  color: color.withOpacity(0.2),
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Icon(icon, color: color, size: 32),
              ),
              const SizedBox(height: 12),
              Text(
                title,
                textAlign: TextAlign.center,
                style: const TextStyle(
                  fontSize: 14,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
              const SizedBox(height: 5),
              Text(
                subtitle,
                style: TextStyle(
                  fontSize: 12,
                  color: Colors.grey[400],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildTrendCard(
    String title,
    String value,
    String subtitle,
    IconData icon,
    Color color,
  ) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(15),
        child: Row(
          children: [
            Container(
              padding: const EdgeInsets.all(10),
              decoration: BoxDecoration(
                color: color.withOpacity(0.2),
                borderRadius: BorderRadius.circular(10),
              ),
              child: Icon(icon, color: color, size: 24),
            ),
            const SizedBox(width: 15),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: const TextStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.w600,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    subtitle,
                    style: TextStyle(
                      fontSize: 12,
                      color: Colors.grey[400],
                    ),
                  ),
                ],
              ),
            ),
            Text(
              value,
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: color,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildSensorActivityRow(String name, int count, Color color) {
    return Row(
      children: [
        Expanded(
          flex: 3,
          child: Text(
            name,
            style: const TextStyle(
              fontSize: 14,
              color: Colors.white,
            ),
          ),
        ),
        Expanded(
          flex: 5,
          child: LinearProgressIndicator(
            value: count / 50,
            backgroundColor: Colors.grey[800],
            valueColor: AlwaysStoppedAnimation<Color>(color),
          ),
        ),
        const SizedBox(width: 10),
        Text(
          '$count',
          style: TextStyle(
            fontSize: 14,
            fontWeight: FontWeight.bold,
            color: color,
          ),
        ),
      ],
    );
  }

  Widget _buildTimeSlotRow(String label, String percentage, double value) {
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
              percentage,
              style: const TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: Color(0xFF0A84FF),
              ),
            ),
          ],
        ),
        const SizedBox(height: 8),
        LinearProgressIndicator(
          value: value,
          backgroundColor: Colors.grey[800],
          valueColor: const AlwaysStoppedAnimation<Color>(Color(0xFF0A84FF)),
        ),
      ],
    );
  }
}

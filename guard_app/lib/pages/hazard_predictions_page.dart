import 'package:flutter/material.dart';
import '../models/hazard_prediction.dart';

class HazardPredictionsPage extends StatelessWidget {
  const HazardPredictionsPage({super.key});

  @override
  Widget build(BuildContext context) {
    final hazards = [
      HazardPrediction(
        title: 'Candle too close to curtain',
        confidence: '91%',
        action: 'Moved curtain via smart actuator',
        status: 'Resolved',
      ),
      HazardPrediction(
        title: 'Child approaching balcony door',
        confidence: '88%',
        action: 'Sent alert to parents\' phones',
        status: 'Awaiting confirmation',
      ),
      HazardPrediction(
        title: 'Overloaded power strip',
        confidence: '94%',
        action: 'Smart plug turned OFF',
        status: 'Prevented',
      ),
      HazardPrediction(
        title: 'Air quality dropping rapidly',
        confidence: '73%',
        action: 'Alerted user — check kitchen',
        status: 'Attention needed',
      ),
      HazardPrediction(
        title: 'Water accumulation detected',
        confidence: '85%',
        action: 'Water valve closed automatically',
        status: 'Resolved',
      ),
      HazardPrediction(
        title: 'Unusual heat spike in outlet',
        confidence: '78%',
        action: 'Circuit breaker triggered',
        status: 'Prevented',
      ),
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Hazard Predictions',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
      ),
      body: ListView.builder(
        padding: const EdgeInsets.all(15),
        itemCount: hazards.length,
        itemBuilder: (context, index) {
          return _buildHazardCard(hazards[index]);
        },
      ),
    );
  }

  Widget _buildHazardCard(HazardPrediction hazard) {
    Color statusColor;
    IconData statusIcon;

    switch (hazard.status) {
      case 'Resolved':
        statusColor = Colors.green;
        statusIcon = Icons.check_circle;
        break;
      case 'Prevented':
        statusColor = Colors.blue;
        statusIcon = Icons.shield;
        break;
      case 'Awaiting confirmation':
        statusColor = Colors.orange;
        statusIcon = Icons.pending;
        break;
      case 'Attention needed':
        statusColor = Colors.red;
        statusIcon = Icons.warning;
        break;
      default:
        statusColor = Colors.grey;
        statusIcon = Icons.info;
    }

    return Card(
      margin: const EdgeInsets.only(bottom: 15),
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Title with icon
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(8),
                  decoration: BoxDecoration(
                    color: statusColor.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Icon(
                    Icons.warning_amber_rounded,
                    color: statusColor,
                    size: 24,
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(
                    hazard.title,
                    style: const TextStyle(
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 15),

            // Confidence meter
            Row(
              children: [
                const Icon(
                  Icons.psychology,
                  size: 18,
                  color: Color(0xFF0A84FF),
                ),
                const SizedBox(width: 8),
                const Text(
                  'AI Confidence: ',
                  style: TextStyle(
                    fontSize: 14,
                    color: Colors.white70,
                  ),
                ),
                Text(
                  hazard.confidence,
                  style: const TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF0A84FF),
                  ),
                ),
                const SizedBox(width: 10),
                Expanded(
                  child: LinearProgressIndicator(
                    value: double.parse(hazard.confidence.replaceAll('%', '')) / 100,
                    backgroundColor: Colors.grey[800],
                    valueColor: const AlwaysStoppedAnimation<Color>(Color(0xFF0A84FF)),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 15),

            // Action taken
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: const Color(0xFF1A1A1A).withOpacity(0.5),
                borderRadius: BorderRadius.circular(8),
                border: Border.all(
                  color: const Color(0xFF0A84FF).withOpacity(0.3),
                ),
              ),
              child: Row(
                children: [
                  const Icon(
                    Icons.auto_fix_high,
                    size: 18,
                    color: Color(0xFF0A84FF),
                  ),
                  const SizedBox(width: 10),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const Text(
                          'Action Taken:',
                          style: TextStyle(
                            fontSize: 12,
                            color: Colors.white70,
                          ),
                        ),
                        const SizedBox(height: 3),
                        Text(
                          hazard.action,
                          style: const TextStyle(
                            fontSize: 14,
                            fontWeight: FontWeight.w600,
                            color: Colors.white,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),

            const SizedBox(height: 15),

            // Status
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Row(
                  children: [
                    Icon(statusIcon, size: 18, color: statusColor),
                    const SizedBox(width: 8),
                    Text(
                      'Status: ',
                      style: TextStyle(
                        fontSize: 14,
                        color: Colors.grey[400],
                      ),
                    ),
                    Text(
                      hazard.status,
                      style: TextStyle(
                        fontSize: 14,
                        fontWeight: FontWeight.bold,
                        color: statusColor,
                      ),
                    ),
                  ],
                ),
                if (hazard.status == 'Awaiting confirmation' ||
                    hazard.status == 'Attention needed')
                  TextButton(
                    onPressed: () {},
                    child: const Text('View Details'),
                  ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

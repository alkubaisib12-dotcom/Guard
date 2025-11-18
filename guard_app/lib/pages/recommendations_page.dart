import 'package:flutter/material.dart';

class RecommendationsPage extends StatefulWidget {
  const RecommendationsPage({super.key});

  @override
  State<RecommendationsPage> createState() => _RecommendationsPageState();
}

class _RecommendationsPageState extends State<RecommendationsPage> {
  late List<Map<String, dynamic>> recommendations;

  @override
  void initState() {
    super.initState();
    recommendations = [
      {
        'title': 'Install a smoke detector in the kitchen',
        'description':
            'Based on recent cooking incidents, a smoke detector would provide faster fire detection and alert you before hazards escalate.',
        'priority': 'High',
        'icon': Icons.smoke_free,
      },
      {
        'title': 'Add a smart lock to the front door',
        'description':
            'Enhanced child safety feature to prevent unsupervised exits. Can be controlled remotely and set on schedules.',
        'priority': 'Medium',
        'icon': Icons.lock,
      },
      {
        'title': 'Use current sensors on bedroom outlets',
        'description':
            'Track power consumption spikes that could indicate electrical issues. Helps prevent overloads during nighttime.',
        'priority': 'High',
        'icon': Icons.electrical_services,
      },
      {
        'title': 'Place water leak sensors under all sinks',
        'description':
            'Early detection of pipe leaks can save thousands in water damage. Recommended for kitchen and bathroom areas.',
        'priority': 'Medium',
        'icon': Icons.water_damage,
      },
      {
        'title': 'Install baby gates near stairs',
        'description':
            'Physical safety measure to complement AI monitoring. Recommended for homes with children under 5 years old.',
        'priority': 'High',
        'icon': Icons.child_care,
      },
      {
        'title': 'Add CO₂ monitor to bedrooms',
        'description':
            'Poor air quality during sleep affects health. Monitor CO₂ levels to ensure proper ventilation in sleeping areas.',
        'priority': 'Low',
        'icon': Icons.air,
      },
    ];
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Safety Recommendations',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
      ),
      body: recommendations.isEmpty
          ? Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.check_circle,
                    size: 80,
                    color: Colors.green.withOpacity(0.5),
                  ),
                  const SizedBox(height: 20),
                  const Text(
                    'All caught up!',
                    style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 10),
                  Text(
                    'No new recommendations at this time',
                    style: TextStyle(
                      fontSize: 14,
                      color: Colors.grey[400],
                    ),
                  ),
                ],
              ),
            )
          : ListView.builder(
              padding: const EdgeInsets.all(15),
              itemCount: recommendations.length,
              itemBuilder: (context, index) {
                return _buildRecommendationCard(recommendations[index], index);
              },
            ),
    );
  }

  Widget _buildRecommendationCard(Map<String, dynamic> recommendation, int index) {
    Color priorityColor;
    switch (recommendation['priority']) {
      case 'High':
        priorityColor = Colors.red;
        break;
      case 'Medium':
        priorityColor = Colors.orange;
        break;
      case 'Low':
        priorityColor = Colors.green;
        break;
      default:
        priorityColor = Colors.grey;
    }

    return Card(
      margin: const EdgeInsets.only(bottom: 15),
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(12),
                  decoration: BoxDecoration(
                    color: const Color(0xFF0A84FF).withOpacity(0.2),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Icon(
                    recommendation['icon'],
                    color: const Color(0xFF0A84FF),
                    size: 28,
                  ),
                ),
                const SizedBox(width: 15),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        recommendation['title'],
                        style: const TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.bold,
                          color: Colors.white,
                        ),
                      ),
                      const SizedBox(height: 5),
                      Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 8,
                          vertical: 3,
                        ),
                        decoration: BoxDecoration(
                          color: priorityColor.withOpacity(0.2),
                          borderRadius: BorderRadius.circular(8),
                          border: Border.all(color: priorityColor, width: 1),
                        ),
                        child: Text(
                          '${recommendation['priority']} Priority',
                          style: TextStyle(
                            fontSize: 11,
                            fontWeight: FontWeight.bold,
                            color: priorityColor,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
            const SizedBox(height: 15),
            Text(
              recommendation['description'],
              style: TextStyle(
                fontSize: 14,
                color: Colors.grey[300],
                height: 1.5,
              ),
            ),
            const SizedBox(height: 15),
            Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                TextButton(
                  onPressed: () {
                    setState(() {
                      recommendations.removeAt(index);
                    });
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(
                        content: Text('Recommendation dismissed'),
                        duration: Duration(seconds: 2),
                      ),
                    );
                  },
                  child: const Text('Dismiss'),
                ),
                const SizedBox(width: 10),
                ElevatedButton.icon(
                  onPressed: () {
                    showDialog(
                      context: context,
                      builder: (context) => AlertDialog(
                        backgroundColor: const Color(0xFF1A1A1A),
                        title: const Text(
                          'Shop for Products',
                          style: TextStyle(color: Color(0xFF0A84FF)),
                        ),
                        content: Text(
                          'Looking for: ${recommendation['title']}\n\nWe\'ll redirect you to our partner store where you can find:\n\n• Top-rated products\n• Guard-compatible devices\n• Professional installation\n• Warranty included',
                          style: const TextStyle(color: Colors.white),
                        ),
                        actions: [
                          TextButton(
                            onPressed: () => Navigator.pop(context),
                            child: const Text('Cancel'),
                          ),
                          ElevatedButton(
                            onPressed: () {
                              Navigator.pop(context);
                              ScaffoldMessenger.of(context).showSnackBar(
                                const SnackBar(
                                  content: Text('Opening store...'),
                                  backgroundColor: Color(0xFF0A84FF),
                                ),
                              );
                            },
                            style: ElevatedButton.styleFrom(
                              backgroundColor: const Color(0xFF0A84FF),
                            ),
                            child: const Text('Continue'),
                          ),
                        ],
                      ),
                    );
                  },
                  icon: const Icon(Icons.shopping_cart, size: 18),
                  label: const Text('Shop Now'),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF0A84FF),
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(8),
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

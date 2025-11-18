import 'package:flutter/material.dart';
import '../models/activity_log.dart';

class ActivityLogPage extends StatelessWidget {
  const ActivityLogPage({super.key});

  @override
  Widget build(BuildContext context) {
    final activities = [
      ActivityLog(
        time: '12:44',
        description: 'Movement detected in hallway (IR sensor)',
        type: 'Info',
      ),
      ActivityLog(
        time: '12:41',
        description: 'Water leak sensor triggered (false alarm)',
        type: 'Warning',
      ),
      ActivityLog(
        time: '12:39',
        description: 'Candle hazard prevented in living room',
        type: 'Alert',
      ),
      ActivityLog(
        time: '12:20',
        description: 'CO₂ slightly elevated, monitoring',
        type: 'Warning',
      ),
      ActivityLog(
        time: '12:05',
        description: 'Power strip overload prevented (desk area)',
        type: 'Alert',
      ),
      ActivityLog(
        time: '11:58',
        description: 'Smart plug turned off automatically',
        type: 'Info',
      ),
      ActivityLog(
        time: '11:45',
        description: 'Front door opened',
        type: 'Info',
      ),
      ActivityLog(
        time: '11:30',
        description: 'System health check completed',
        type: 'Info',
      ),
      ActivityLog(
        time: '11:15',
        description: 'Child detected near balcony - Alert sent',
        type: 'Alert',
      ),
      ActivityLog(
        time: '11:00',
        description: 'All sensors online and operational',
        type: 'Info',
      ),
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Activity Log',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.filter_list, color: Color(0xFF0A84FF)),
            onPressed: () {
              showDialog(
                context: context,
                builder: (context) => AlertDialog(
                  backgroundColor: const Color(0xFF1A1A1A),
                  title: const Text(
                    'Filter Activity Log',
                    style: TextStyle(color: Color(0xFF0A84FF)),
                  ),
                  content: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      _buildFilterOption(context, 'All Activities'),
                      _buildFilterOption(context, 'Alerts Only'),
                      _buildFilterOption(context, 'Warnings Only'),
                      _buildFilterOption(context, 'Info Only'),
                    ],
                  ),
                  actions: [
                    TextButton(
                      onPressed: () => Navigator.pop(context),
                      child: const Text('Close'),
                    ),
                  ],
                ),
              );
            },
          ),
        ],
      ),
      body: ListView.builder(
        padding: const EdgeInsets.all(15),
        itemCount: activities.length,
        itemBuilder: (context, index) {
          return _buildActivityItem(activities[index], index == 0);
        },
      ),
    );
  }

  Widget _buildActivityItem(ActivityLog activity, bool isFirst) {
    Color typeColor;
    IconData typeIcon;
    Color chipColor;

    switch (activity.type) {
      case 'Alert':
        typeColor = Colors.red;
        typeIcon = Icons.warning;
        chipColor = Colors.red;
        break;
      case 'Warning':
        typeColor = Colors.orange;
        typeIcon = Icons.info;
        chipColor = Colors.orange;
        break;
      case 'Info':
        typeColor = const Color(0xFF0A84FF);
        typeIcon = Icons.check_circle;
        chipColor = const Color(0xFF0A84FF);
        break;
      default:
        typeColor = Colors.grey;
        typeIcon = Icons.circle;
        chipColor = Colors.grey;
    }

    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Timeline
        Column(
          children: [
            Container(
              width: 40,
              height: 40,
              decoration: BoxDecoration(
                color: typeColor.withOpacity(0.2),
                shape: BoxShape.circle,
                border: Border.all(color: typeColor, width: 2),
              ),
              child: Icon(typeIcon, color: typeColor, size: 20),
            ),
            if (!isFirst)
              Container(
                width: 2,
                height: 60,
                color: Colors.grey[800],
              ),
          ],
        ),
        const SizedBox(width: 15),
        // Content
        Expanded(
          child: Padding(
            padding: const EdgeInsets.only(bottom: 20),
            child: Card(
              child: Padding(
                padding: const EdgeInsets.all(15),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Container(
                          padding: const EdgeInsets.symmetric(
                            horizontal: 10,
                            vertical: 4,
                          ),
                          decoration: BoxDecoration(
                            color: chipColor.withOpacity(0.2),
                            borderRadius: BorderRadius.circular(12),
                            border: Border.all(color: chipColor, width: 1),
                          ),
                          child: Text(
                            activity.type,
                            style: TextStyle(
                              fontSize: 11,
                              fontWeight: FontWeight.bold,
                              color: chipColor,
                            ),
                          ),
                        ),
                        Text(
                          activity.time,
                          style: TextStyle(
                            fontSize: 13,
                            color: Colors.grey[400],
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 10),
                    Text(
                      activity.description,
                      style: const TextStyle(
                        fontSize: 14,
                        color: Colors.white,
                        height: 1.4,
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildFilterOption(BuildContext context, String label) {
    return ListTile(
      title: Text(label, style: const TextStyle(color: Colors.white)),
      onTap: () {
        Navigator.pop(context);
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Filter applied: $label'),
            backgroundColor: const Color(0xFF0A84FF),
            duration: const Duration(seconds: 2),
          ),
        );
      },
    );
  }
}

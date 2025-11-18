import 'package:flutter/material.dart';
import '../models/automation.dart';

class AutomationsPage extends StatefulWidget {
  const AutomationsPage({super.key});

  @override
  State<AutomationsPage> createState() => _AutomationsPageState();
}

class _AutomationsPageState extends State<AutomationsPage> {
  final List<Automation> automations = [
    Automation(
      title: 'Turn off plug when overloaded',
      description: 'Automatically shut down smart plugs when current exceeds safe threshold',
      isEnabled: true,
    ),
    Automation(
      title: 'Move curtain if fire-risk object detected',
      description: 'Smart actuator moves curtains away from heat sources like candles',
      isEnabled: true,
    ),
    Automation(
      title: 'Trigger siren if baby close to balcony',
      description: 'Sound alarm and send alerts when child approaches dangerous areas',
      isEnabled: true,
    ),
    Automation(
      title: 'Send SMS backup alert',
      description: 'Send text message if user doesn\'t respond to app notifications within 2 minutes',
      isEnabled: true,
    ),
    Automation(
      title: 'Lock front door at 11 PM',
      description: 'Automatically engage smart lock every night for security',
      isEnabled: false,
    ),
    Automation(
      title: 'Close water valve on leak',
      description: 'Shut off main water supply when leak sensors detect moisture',
      isEnabled: true,
    ),
    Automation(
      title: 'Turn on lights when motion detected',
      description: 'Activate lighting in hallways and entrances during nighttime movement',
      isEnabled: false,
    ),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Automations',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.add, color: Color(0xFF0A84FF)),
            onPressed: () {
              _showAddAutomationDialog();
            },
          ),
        ],
      ),
      body: Column(
        children: [
          // Summary Banner
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(20),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
                colors: [
                  const Color(0xFF0A84FF).withOpacity(0.2),
                  Colors.transparent,
                ],
              ),
            ),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: [
                _buildSummaryItem(
                  'Total',
                  automations.length.toString(),
                  Icons.auto_awesome,
                ),
                _buildSummaryItem(
                  'Active',
                  automations.where((a) => a.isEnabled).length.toString(),
                  Icons.check_circle,
                ),
                _buildSummaryItem(
                  'Inactive',
                  automations.where((a) => !a.isEnabled).length.toString(),
                  Icons.cancel,
                ),
              ],
            ),
          ),

          // Automations List
          Expanded(
            child: ListView.builder(
              padding: const EdgeInsets.all(15),
              itemCount: automations.length,
              itemBuilder: (context, index) {
                return _buildAutomationCard(automations[index], index);
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSummaryItem(String label, String value, IconData icon) {
    return Column(
      children: [
        Icon(icon, color: const Color(0xFF0A84FF), size: 28),
        const SizedBox(height: 8),
        Text(
          value,
          style: const TextStyle(
            fontSize: 24,
            fontWeight: FontWeight.bold,
            color: Colors.white,
          ),
        ),
        Text(
          label,
          style: TextStyle(
            fontSize: 12,
            color: Colors.grey[400],
          ),
        ),
      ],
    );
  }

  Widget _buildAutomationCard(Automation automation, int index) {
    return Card(
      margin: const EdgeInsets.only(bottom: 15),
      child: Padding(
        padding: const EdgeInsets.all(15),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  width: 40,
                  height: 40,
                  decoration: BoxDecoration(
                    color: automation.isEnabled
                        ? const Color(0xFF0A84FF).withOpacity(0.2)
                        : Colors.grey.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Icon(
                    Icons.auto_awesome,
                    color: automation.isEnabled ? const Color(0xFF0A84FF) : Colors.grey,
                    size: 24,
                  ),
                ),
                const SizedBox(width: 15),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        automation.title,
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
                          color: automation.isEnabled
                              ? Colors.green.withOpacity(0.2)
                              : Colors.grey.withOpacity(0.2),
                          borderRadius: BorderRadius.circular(8),
                        ),
                        child: Text(
                          automation.isEnabled ? 'ACTIVE' : 'INACTIVE',
                          style: TextStyle(
                            fontSize: 10,
                            fontWeight: FontWeight.bold,
                            color: automation.isEnabled ? Colors.green : Colors.grey,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                Switch(
                  value: automation.isEnabled,
                  activeColor: const Color(0xFF0A84FF),
                  onChanged: (value) {
                    setState(() {
                      automation.isEnabled = value;
                    });
                  },
                ),
              ],
            ),
            const SizedBox(height: 15),
            Text(
              automation.description,
              style: TextStyle(
                fontSize: 13,
                color: Colors.grey[300],
                height: 1.4,
              ),
            ),
            const SizedBox(height: 15),
            Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                TextButton.icon(
                  onPressed: () {
                    _showEditDialog(automation);
                  },
                  icon: const Icon(Icons.edit, size: 16),
                  label: const Text('Edit'),
                  style: TextButton.styleFrom(
                    foregroundColor: const Color(0xFF0A84FF),
                  ),
                ),
                TextButton.icon(
                  onPressed: () {
                    _showDeleteDialog(index);
                  },
                  icon: const Icon(Icons.delete, size: 16),
                  label: const Text('Delete'),
                  style: TextButton.styleFrom(
                    foregroundColor: Colors.red,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  void _showAddAutomationDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: const Color(0xFF1A1A1A),
        title: const Text(
          'Add New Automation',
          style: TextStyle(color: Color(0xFF0A84FF)),
        ),
        content: const Text(
          'This feature allows you to create custom automation rules. Configure triggers, conditions, and actions.',
          style: TextStyle(color: Colors.white),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () {
              Navigator.pop(context);
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: const Color(0xFF0A84FF),
            ),
            child: const Text('Create'),
          ),
        ],
      ),
    );
  }

  void _showEditDialog(Automation automation) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: const Color(0xFF1A1A1A),
        title: const Text(
          'Edit Automation',
          style: TextStyle(color: Color(0xFF0A84FF)),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              automation.title,
              style: const TextStyle(
                color: Colors.white,
                fontWeight: FontWeight.bold,
                fontSize: 16,
              ),
            ),
            const SizedBox(height: 15),
            Text(
              automation.description,
              style: const TextStyle(color: Colors.white70, height: 1.4),
            ),
            const SizedBox(height: 20),
            const Text(
              'Configure triggers, conditions, and actions for this automation rule.',
              style: TextStyle(color: Color(0xFF0A84FF)),
            ),
          ],
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
                  content: Text('Automation settings updated'),
                  backgroundColor: Color(0xFF0A84FF),
                ),
              );
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: const Color(0xFF0A84FF),
            ),
            child: const Text('Save Changes'),
          ),
        ],
      ),
    );
  }

  void _showDeleteDialog(int index) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: const Color(0xFF1A1A1A),
        title: const Text(
          'Delete Automation',
          style: TextStyle(color: Colors.red),
        ),
        content: const Text(
          'Are you sure you want to delete this automation rule?',
          style: TextStyle(color: Colors.white),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () {
              setState(() {
                automations.removeAt(index);
              });
              Navigator.pop(context);
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.red,
            ),
            child: const Text('Delete'),
          ),
        ],
      ),
    );
  }
}

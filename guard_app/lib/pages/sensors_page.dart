import 'package:flutter/material.dart';
import '../models/sensor.dart';

class SensorsPage extends StatelessWidget {
  const SensorsPage({super.key});

  @override
  Widget build(BuildContext context) {
    final sensors = [
      Sensor(
        name: 'Air Quality Sensor',
        status: 'WARNING',
        details: 'Kitchen • CO₂ = 900 ppm',
        icon: 'air',
      ),
      Sensor(
        name: 'Current Sensor',
        status: 'NORMAL',
        details: 'Living Room • 0.9 A',
        icon: 'bolt',
      ),
      Sensor(
        name: 'Water Leak Sensor',
        status: 'SAFE',
        details: 'Bathroom • No moisture detected',
        icon: 'water',
      ),
      Sensor(
        name: 'IR Motion Sensor',
        status: 'ACTIVE',
        details: 'Hallway • Movement at 12:44',
        icon: 'motion',
      ),
      Sensor(
        name: 'Smart Plug Monitor',
        status: 'AUTO-OFF',
        details: 'Workstation • Overload prevented',
        icon: 'plug',
      ),
      Sensor(
        name: 'Temperature Sensor',
        status: 'NORMAL',
        details: 'Bedroom • 22°C',
        icon: 'temperature',
      ),
      Sensor(
        name: 'Smoke Detector',
        status: 'SAFE',
        details: 'Kitchen • No smoke detected',
        icon: 'smoke',
      ),
      Sensor(
        name: 'Door Sensor',
        status: 'OPEN',
        details: 'Front Door • Opened at 13:15',
        icon: 'door',
      ),
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Sensors Overview',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.filter_list, color: Color(0xFF0A84FF)),
            onPressed: () {
              _showFilterDialog(context);
            },
          ),
        ],
      ),
      body: ListView.builder(
        padding: const EdgeInsets.all(15),
        itemCount: sensors.length,
        itemBuilder: (context, index) {
          return _buildSensorCard(sensors[index]);
        },
      ),
    );
  }

  Widget _buildSensorCard(Sensor sensor) {
    Color statusColor;
    IconData statusIcon;

    switch (sensor.status) {
      case 'WARNING':
        statusColor = Colors.orange;
        statusIcon = Icons.warning_amber_rounded;
        break;
      case 'SAFE':
      case 'NORMAL':
        statusColor = Colors.green;
        statusIcon = Icons.check_circle;
        break;
      case 'ACTIVE':
        statusColor = const Color(0xFF0A84FF);
        statusIcon = Icons.sensors;
        break;
      case 'AUTO-OFF':
        statusColor = Colors.purple;
        statusIcon = Icons.shield;
        break;
      case 'OPEN':
        statusColor = Colors.yellow;
        statusIcon = Icons.info;
        break;
      default:
        statusColor = Colors.grey;
        statusIcon = Icons.help;
    }

    IconData sensorIcon;
    switch (sensor.icon) {
      case 'air':
        sensorIcon = Icons.air;
        break;
      case 'bolt':
        sensorIcon = Icons.bolt;
        break;
      case 'water':
        sensorIcon = Icons.water_drop;
        break;
      case 'motion':
        sensorIcon = Icons.motion_photos_on;
        break;
      case 'plug':
        sensorIcon = Icons.power;
        break;
      case 'temperature':
        sensorIcon = Icons.thermostat;
        break;
      case 'smoke':
        sensorIcon = Icons.smoke_free;
        break;
      case 'door':
        sensorIcon = Icons.door_front_door;
        break;
      default:
        sensorIcon = Icons.sensors;
    }

    return Card(
      margin: const EdgeInsets.only(bottom: 15),
      child: ListTile(
        contentPadding: const EdgeInsets.all(15),
        leading: Container(
          width: 50,
          height: 50,
          decoration: BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
              colors: [
                statusColor.withOpacity(0.3),
                statusColor.withOpacity(0.1),
              ],
            ),
            borderRadius: BorderRadius.circular(12),
          ),
          child: Icon(sensorIcon, color: statusColor, size: 28),
        ),
        title: Text(
          sensor.name,
          style: const TextStyle(
            fontSize: 16,
            fontWeight: FontWeight.bold,
            color: Colors.white,
          ),
        ),
        subtitle: Padding(
          padding: const EdgeInsets.only(top: 8),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                sensor.details,
                style: TextStyle(
                  fontSize: 13,
                  color: Colors.grey[400],
                ),
              ),
              const SizedBox(height: 8),
              Row(
                children: [
                  Icon(statusIcon, size: 14, color: statusColor),
                  const SizedBox(width: 5),
                  Text(
                    sensor.status,
                    style: TextStyle(
                      fontSize: 12,
                      fontWeight: FontWeight.w600,
                      color: statusColor,
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
        trailing: IconButton(
          icon: Icon(Icons.more_vert, color: Colors.grey[600]),
          onPressed: () {
            showModalBottomSheet(
              context: context,
              backgroundColor: const Color(0xFF1A1A1A),
              builder: (context) => SafeArea(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    ListTile(
                      leading: const Icon(Icons.info, color: Color(0xFF0A84FF)),
                      title: const Text('View Details', style: TextStyle(color: Colors.white)),
                      onTap: () {
                        Navigator.pop(context);
                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(content: Text('Sensor details coming soon')),
                        );
                      },
                    ),
                    ListTile(
                      leading: const Icon(Icons.edit, color: Color(0xFF0A84FF)),
                      title: const Text('Edit Sensor', style: TextStyle(color: Colors.white)),
                      onTap: () {
                        Navigator.pop(context);
                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(content: Text('Edit sensor coming soon')),
                        );
                      },
                    ),
                    ListTile(
                      leading: const Icon(Icons.delete, color: Colors.red),
                      title: const Text('Remove Sensor', style: TextStyle(color: Colors.white)),
                      onTap: () {
                        Navigator.pop(context);
                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(content: Text('Remove sensor coming soon')),
                        );
                      },
                    ),
                  ],
                ),
              ),
            );
          },
        ),
      ),
    );
  }

  void _showFilterDialog(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: const Color(0xFF1A1A1A),
        title: const Text(
          'Filter Sensors',
          style: TextStyle(color: Color(0xFF0A84FF)),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            _buildFilterOption('All Sensors'),
            _buildFilterOption('Warnings Only'),
            _buildFilterOption('Active Only'),
            _buildFilterOption('Safe Only'),
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
  }

  Widget _buildFilterOption(String label) {
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

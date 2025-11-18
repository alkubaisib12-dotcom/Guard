import 'package:flutter/material.dart';
import '../models/room.dart';

class HouseMapPage extends StatelessWidget {
  const HouseMapPage({super.key});

  @override
  Widget build(BuildContext context) {
    final rooms = [
      Room(
        name: 'Living Room',
        status: 'Safe',
        statusText: 'All systems normal',
        statusColor: Colors.green,
      ),
      Room(
        name: 'Kitchen',
        status: 'Warning',
        statusText: 'Air Quality - CO₂ elevated',
        statusColor: Colors.orange,
      ),
      Room(
        name: 'Bedroom',
        status: 'Safe',
        statusText: 'All systems normal',
        statusColor: Colors.green,
      ),
      Room(
        name: 'Entrance',
        status: 'Safe',
        statusText: 'All systems normal',
        statusColor: Colors.green,
      ),
      Room(
        name: 'Balcony',
        status: 'Hazard',
        statusText: 'Child detected near edge',
        statusColor: Colors.red,
      ),
      Room(
        name: 'Bathroom',
        status: 'Safe',
        statusText: 'No leaks detected',
        statusColor: Colors.green,
      ),
      Room(
        name: 'Kids Room',
        status: 'Safe',
        statusText: 'Monitoring active',
        statusColor: Colors.green,
      ),
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'House Map',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh, color: Color(0xFF0A84FF)),
            onPressed: () {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(
                  content: Text('Refreshing room status...'),
                  backgroundColor: Color(0xFF0A84FF),
                  duration: Duration(seconds: 2),
                ),
              );
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
            child: Column(
              children: [
                const Text(
                  'Home Status Overview',
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
                const SizedBox(height: 15),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: [
                    _buildStatusIndicator(
                      'Safe',
                      rooms.where((r) => r.status == 'Safe').length.toString(),
                      Colors.green,
                    ),
                    _buildStatusIndicator(
                      'Warning',
                      rooms.where((r) => r.status == 'Warning').length.toString(),
                      Colors.orange,
                    ),
                    _buildStatusIndicator(
                      'Hazard',
                      rooms.where((r) => r.status == 'Hazard').length.toString(),
                      Colors.red,
                    ),
                  ],
                ),
              ],
            ),
          ),

          // Room Cards
          Expanded(
            child: ListView.builder(
              padding: const EdgeInsets.all(15),
              itemCount: rooms.length,
              itemBuilder: (context, index) {
                return _buildRoomCard(rooms[index]);
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStatusIndicator(String label, String count, Color color) {
    return Column(
      children: [
        Container(
          width: 50,
          height: 50,
          decoration: BoxDecoration(
            color: color.withOpacity(0.2),
            shape: BoxShape.circle,
            border: Border.all(color: color, width: 2),
          ),
          child: Center(
            child: Text(
              count,
              style: TextStyle(
                fontSize: 20,
                fontWeight: FontWeight.bold,
                color: color,
              ),
            ),
          ),
        ),
        const SizedBox(height: 8),
        Text(
          label,
          style: const TextStyle(
            fontSize: 12,
            color: Colors.white70,
          ),
        ),
      ],
    );
  }

  Widget _buildRoomCard(Room room) {
    IconData roomIcon;
    switch (room.name) {
      case 'Living Room':
        roomIcon = Icons.weekend;
        break;
      case 'Kitchen':
        roomIcon = Icons.kitchen;
        break;
      case 'Bedroom':
        roomIcon = Icons.bed;
        break;
      case 'Entrance':
        roomIcon = Icons.door_front_door;
        break;
      case 'Balcony':
        roomIcon = Icons.balcony;
        break;
      case 'Bathroom':
        roomIcon = Icons.bathroom;
        break;
      case 'Kids Room':
        roomIcon = Icons.child_care;
        break;
      default:
        roomIcon = Icons.home;
    }

    return Card(
      margin: const EdgeInsets.only(bottom: 15),
      child: ListTile(
        contentPadding: const EdgeInsets.all(15),
        leading: CircleAvatar(
          radius: 25,
          backgroundColor: room.statusColor.withOpacity(0.2),
          child: Icon(roomIcon, color: room.statusColor, size: 28),
        ),
        title: Text(
          room.name,
          style: const TextStyle(
            fontSize: 18,
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
                room.statusText,
                style: TextStyle(
                  fontSize: 13,
                  color: Colors.grey[400],
                ),
              ),
              const SizedBox(height: 8),
              Row(
                children: [
                  Container(
                    width: 8,
                    height: 8,
                    decoration: BoxDecoration(
                      color: room.statusColor,
                      shape: BoxShape.circle,
                    ),
                  ),
                  const SizedBox(width: 8),
                  Text(
                    room.status,
                    style: TextStyle(
                      fontSize: 13,
                      fontWeight: FontWeight.bold,
                      color: room.statusColor,
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
        trailing: Icon(
          Icons.arrow_forward_ios,
          color: Colors.grey[600],
          size: 18,
        ),
        onTap: () {
          showDialog(
            context: context,
            builder: (context) => AlertDialog(
              backgroundColor: const Color(0xFF1A1A1A),
              title: Text(
                room.name,
                style: TextStyle(color: room.statusColor),
              ),
              content: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Icon(Icons.info, color: room.statusColor, size: 20),
                      const SizedBox(width: 10),
                      Text(
                        'Status: ${room.status}',
                        style: TextStyle(
                          color: room.statusColor,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 15),
                  Text(
                    room.statusText,
                    style: const TextStyle(color: Colors.white),
                  ),
                  const SizedBox(height: 20),
                  const Text(
                    'Connected Devices:',
                    style: TextStyle(
                      color: Color(0xFF0A84FF),
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 10),
                  const Text(
                    '• Camera: Active\n• Motion Sensor: Online\n• Smart Plug: Connected',
                    style: TextStyle(color: Colors.white70, height: 1.5),
                  ),
                ],
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
                        content: Text('Managing room settings...'),
                        backgroundColor: Color(0xFF0A84FF),
                      ),
                    );
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF0A84FF),
                  ),
                  child: const Text('Manage'),
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}

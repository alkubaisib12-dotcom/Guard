import 'package:flutter/material.dart';
import '../models/camera.dart';
import 'live_hazard_page.dart';

class CamerasPage extends StatelessWidget {
  const CamerasPage({super.key});

  @override
  Widget build(BuildContext context) {
    final cameras = [
      Camera(
        name: 'Living Room Camera',
        location: 'Living Room - North Corner',
        status: 'Active',
      ),
      Camera(
        name: 'Entrance Camera',
        location: 'Main Entrance',
        status: 'Active',
      ),
      Camera(
        name: 'Kids Room Camera',
        location: 'Kids Room - Above Crib',
        status: 'Active',
      ),
      Camera(
        name: 'Kitchen Camera',
        location: 'Kitchen - Ceiling',
        status: 'Active',
      ),
      Camera(
        name: 'Balcony Camera',
        location: 'Balcony - Door View',
        status: 'Hazard Detected',
      ),
    ];

    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Live Cameras',
          style: TextStyle(
            fontWeight: FontWeight.bold,
            color: Color(0xFF0A84FF),
          ),
        ),
      ),
      body: Padding(
        padding: const EdgeInsets.all(15),
        child: GridView.builder(
          gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
            crossAxisCount: 2,
            crossAxisSpacing: 15,
            mainAxisSpacing: 15,
            childAspectRatio: 0.85,
          ),
          itemCount: cameras.length,
          itemBuilder: (context, index) {
            final camera = cameras[index];
            return _buildCameraCard(context, camera);
          },
        ),
      ),
    );
  }

  Widget _buildCameraCard(BuildContext context, Camera camera) {
    final isHazard = camera.status == 'Hazard Detected';

    return InkWell(
      onTap: () {
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => LiveHazardPage(camera: camera),
          ),
        );
      },
      child: Card(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Camera preview (placeholder)
            Expanded(
              child: Container(
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                    colors: [
                      const Color(0xFF1A1A1A),
                      isHazard
                        ? Colors.red.withOpacity(0.3)
                        : const Color(0xFF0A84FF).withOpacity(0.2),
                    ],
                  ),
                  borderRadius: const BorderRadius.only(
                    topLeft: Radius.circular(15),
                    topRight: Radius.circular(15),
                  ),
                ),
                child: Stack(
                  children: [
                    Center(
                      child: Icon(
                        Icons.videocam,
                        size: 50,
                        color: Colors.white.withOpacity(0.5),
                      ),
                    ),
                    if (isHazard)
                      Positioned(
                        top: 10,
                        right: 10,
                        child: Container(
                          padding: const EdgeInsets.symmetric(
                            horizontal: 8,
                            vertical: 4,
                          ),
                          decoration: BoxDecoration(
                            color: Colors.red,
                            borderRadius: BorderRadius.circular(12),
                          ),
                          child: const Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Icon(Icons.warning, size: 12, color: Colors.white),
                              SizedBox(width: 4),
                              Text(
                                'ALERT',
                                style: TextStyle(
                                  fontSize: 10,
                                  fontWeight: FontWeight.bold,
                                  color: Colors.white,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    Positioned(
                      top: 10,
                      left: 10,
                      child: Container(
                        width: 8,
                        height: 8,
                        decoration: const BoxDecoration(
                          color: Colors.green,
                          shape: BoxShape.circle,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
            // Camera info
            Padding(
              padding: const EdgeInsets.all(12),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    camera.name,
                    style: const TextStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                  const SizedBox(height: 4),
                  Text(
                    camera.location,
                    style: TextStyle(
                      fontSize: 11,
                      color: Colors.grey[400],
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                  const SizedBox(height: 8),
                  Row(
                    children: [
                      Icon(
                        isHazard ? Icons.warning : Icons.check_circle,
                        size: 14,
                        color: isHazard ? Colors.red : Colors.green,
                      ),
                      const SizedBox(width: 4),
                      Expanded(
                        child: Text(
                          camera.status,
                          style: TextStyle(
                            fontSize: 11,
                            fontWeight: FontWeight.w600,
                            color: isHazard ? Colors.red : Colors.green,
                          ),
                          maxLines: 1,
                          overflow: TextOverflow.ellipsis,
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

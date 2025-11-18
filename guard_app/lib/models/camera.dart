class Camera {
  final String name;
  final String location;
  final String status;
  final String? imagePath;

  Camera({
    required this.name,
    required this.location,
    required this.status,
    this.imagePath,
  });
}

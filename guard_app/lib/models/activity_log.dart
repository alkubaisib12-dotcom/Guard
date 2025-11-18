class ActivityLog {
  final String time;
  final String description;
  final String type; // Alert, Info, Warning

  ActivityLog({
    required this.time,
    required this.description,
    required this.type,
  });
}

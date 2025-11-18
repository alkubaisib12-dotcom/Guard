class Automation {
  final String title;
  final String description;
  bool isEnabled;

  Automation({
    required this.title,
    required this.description,
    this.isEnabled = true,
  });
}

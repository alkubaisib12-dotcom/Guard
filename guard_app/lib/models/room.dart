import 'package:flutter/material.dart';

class Room {
  final String name;
  final String status;
  final String statusText;
  final Color statusColor;

  Room({
    required this.name,
    required this.status,
    required this.statusText,
    required this.statusColor,
  });
}

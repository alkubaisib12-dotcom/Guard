import 'package:shared_preferences/shared_preferences.dart';
import 'dart:convert';

class StorageService {
  static late SharedPreferences _prefs;

  static Future<void> init() async {
    _prefs = await SharedPreferences.getInstance();
  }

  // Auth
  static Future<void> setLoggedIn(bool value) async {
    await _prefs.setBool('isLoggedIn', value);
  }

  static bool isLoggedIn() {
    return _prefs.getBool('isLoggedIn') ?? false;
  }

  static Future<void> setUserName(String name) async {
    await _prefs.setString('userName', name);
  }

  static String getUserName() {
    return _prefs.getString('userName') ?? 'User';
  }

  static Future<void> setUserEmail(String email) async {
    await _prefs.setString('userEmail', email);
  }

  static String getUserEmail() {
    return _prefs.getString('userEmail') ?? '';
  }

  static Future<void> logout() async {
    await _prefs.clear();
  }

  // Settings
  static Future<void> setBool(String key, bool value) async {
    await _prefs.setBool(key, value);
  }

  static bool getBool(String key, {bool defaultValue = false}) {
    return _prefs.getBool(key) ?? defaultValue;
  }

  static Future<void> setString(String key, String value) async {
    await _prefs.setString(key, value);
  }

  static String getString(String key, {String defaultValue = ''}) {
    return _prefs.getString(key) ?? defaultValue;
  }

  // Sensors
  static Future<void> saveSensors(List<Map<String, dynamic>> sensors) async {
    await _prefs.setString('sensors', json.encode(sensors));
  }

  static List<Map<String, dynamic>> getSensors() {
    final String? data = _prefs.getString('sensors');
    if (data == null) return [];
    return List<Map<String, dynamic>>.from(json.decode(data));
  }

  // Cameras
  static Future<void> saveCameras(List<Map<String, dynamic>> cameras) async {
    await _prefs.setString('cameras', json.encode(cameras));
  }

  static List<Map<String, dynamic>> getCameras() {
    final String? data = _prefs.getString('cameras');
    if (data == null) return [];
    return List<Map<String, dynamic>>.from(json.decode(data));
  }

  // Automations
  static Future<void> saveAutomations(List<Map<String, dynamic>> automations) async {
    await _prefs.setString('automations', json.encode(automations));
  }

  static List<Map<String, dynamic>> getAutomations() {
    final String? data = _prefs.getString('automations');
    if (data == null) return [];
    return List<Map<String, dynamic>>.from(json.decode(data));
  }

  // Rooms
  static Future<void> saveRooms(List<Map<String, dynamic>> rooms) async {
    await _prefs.setString('rooms', json.encode(rooms));
  }

  static List<Map<String, dynamic>> getRooms() {
    final String? data = _prefs.getString('rooms');
    if (data == null) return [];
    return List<Map<String, dynamic>>.from(json.decode(data));
  }
}

import 'dart:io';

import 'package:flutter/material.dart';
import 'package:game_mock/home_page.dart';

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Game Mock',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.teal),
        useMaterial3: true,
      ),
      home: HomePage(title: 'Game Mock - ${_getPlatformText()}'),
    );
  }

  String _getPlatformText() {
    if (Platform.isWindows) {
      return 'Windows';
    } else if (Platform.isAndroid) {
      return 'Android';
    } else {
      return 'Other';
    }
  }
}

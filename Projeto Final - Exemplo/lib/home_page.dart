import 'dart:convert';
import 'dart:io';
import 'package:dart_amqp/dart_amqp.dart';
import 'package:device_info_plus/device_info_plus.dart';
import 'package:game_mock/helper.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';
import 'package:path_provider/path_provider.dart';

import 'package:flutter/material.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key, required this.title});

  final String title;

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  bool isSaving = false;
  DeviceInfoPlugin deviceInfo = DeviceInfoPlugin();

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance
        .addPostFrameCallback((_) async => await configureSubscription());
  }

  ButtonStyle get _buttonStyle {
    return ButtonStyle(
        backgroundColor: MaterialStateProperty.all<Color>(
      Theme.of(context).colorScheme.inversePrimary,
    ));
  }

  Future<String> get _filePath async {
    final directory = Platform.isWindows
        ? 'saves'
        : (await getApplicationDocumentsDirectory()).path;

    return '$directory/save.psav';
  }

  Future<Map?> get _fileContent async {
    File file = File(await _filePath);

    if (await file.exists()) {
      final str = await file.readAsString();
      final json = Helper.serialize(str);
      return jsonDecode(json);
    }
    return null;
  }

  Future<String> get _deviceId async {
    if (Platform.isWindows) {
      var windowsInfo = await deviceInfo.windowsInfo;
      return windowsInfo.deviceId;
    } else {
      var androidInfo = await deviceInfo.androidInfo;
      return androidInfo.id;
    }
  }

  Future configureSubscription() async {
    final host = Platform.isWindows ? 'localhost' : '10.0.2.2';
    final platform = Platform.isWindows ? 'PC' : 'Android';

    ConnectionSettings settings = ConnectionSettings(host: host);

    Client client = Client(settings: settings);

    Channel channel = await client.channel();
    final exchange = await channel.exchange(
      "SaveVault",
      ExchangeType.TOPIC,
      durable: true,
    );

    const gameUserId =
        '991a7d20-aefc-461c-8c84-6cd7d7e3f82a.cd867ab2-4b30-48c9-ae0f-6456279705d2';
    final deviceId = await _deviceId;

    debugPrint("Waiting for messages");

    final consumer = await exchange.bindQueueConsumer(
      '$platform.$gameUserId.$deviceId',
      [gameUserId],
      consumerTag: '$platform.$gameUserId.$deviceId',
      durable: true,
      noAck: false,
    );

    consumer.listen((AmqpMessage message) async {
      debugPrint('Message received: ${message.payloadAsString}');
      await downloadSave();
      message.ack();
    });
  }

  Future deleteSave() async {
    try {
      File file = File(await _filePath);
      if (await file.exists()) {
        await file.delete();
        showSnack('Arquivo apagado', Colors.green);
      } else {
        showSnack('Arquivo não existe', Colors.amber);
      }

      setState(() {});
    } catch (ex) {
      showSnack('Erro ao excluir arquivo', Colors.red);
    }
  }

  Future createSave({withSync = false}) async {
    try {
      File file = File(await _filePath);
      final timestamp = DateTime.now().toUtc().toIso8601String();
      final platform = Platform.isWindows ? 'PC' : 'Android';

      String content =
          'user-id = \'991a7d20-aefc-461c-8c84-6cd7d7e3f82a\',\ngame-id = \'cd867ab2-4b30-48c9-ae0f-6456279705d2\',\nplatform = \'$platform\',\ntimestamp = \'$timestamp\',\naccessed-content = [\'23815979-e9c5-45df-9aa3-d8acd219996b\'],\ndata = {\n\tmainPlayerName = \'Player\'\n}\n';
      await file.writeAsString(content);

      isSaving = true;

      if (withSync) {
        await uploadSave([file], withSync: true);
      }

      isSaving = false;

      setState(() {});
      showSnack('Salvo com sucesso', Colors.green);
    } catch (ex) {
      showSnack('Erro ao salvar', Colors.red);
    }
  }

  Future uploadSave(List<File> files, {bool withSync = false}) async {
    final host = Platform.isWindows ? 'localhost' : '10.0.2.2';
    String endpoint = 'Upload';

    if (withSync) {
      endpoint += '/UploadWithSync';
    }

    final request = http.MultipartRequest(
      'POST',
      Uri.parse('https://$host:7041/$endpoint'),
    );

    for (File file in files) {
      final fileStream = http.ByteStream(Stream.castFrom(file.openRead()));
      final length = await file.length();

      final multipartFile = http.MultipartFile(
        'files',
        fileStream,
        length,
        filename: file.path.split('/').last,
      );

      request.files.add(multipartFile);
    }

    final response = await request.send();

    if (!withSync) {
      if (response.statusCode == 200) {
        showSnack('Arquivo de salvamento enviado', Colors.green);
      } else {
        showSnack('Erro ao enviar arquivo de salvamento', Colors.red);
      }
    }
  }

  Future downloadSave() async {
    if (isSaving) {
      return;
    }

    final host = Platform.isWindows ? 'localhost' : '10.0.2.2';
    final platform = Platform.isWindows ? 'PC' : 'Android';
    final body = jsonEncode({
      'TargetPlatform': platform,
      'GameId': 'cd867ab2-4b30-48c9-ae0f-6456279705d2',
      'UserId': '991a7d20-aefc-461c-8c84-6cd7d7e3f82a'
    });

    final response = await http.post(
      Uri.parse('https://$host:7041/Download/Latest/'),
      headers: {'Content-Type': 'application/json'},
      body: body,
    );

    if (response.statusCode == 200) {
      File file = File(await _filePath);
      await file.writeAsBytes(response.bodyBytes);
      setState(() {});
      showSnack('Arquivo de salvamento baixado', Colors.green);
    } else {
      showSnack('Erro ao baixar arquivo de salvamento', Colors.red);
    }
  }

  void showSnack(String message, Color color) {
    final snackBar = SnackBar(
      content: Text(message),
      backgroundColor: color,
    );

    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }

  Text buildRichText(String title, String content) {
    return Text.rich(
      TextSpan(
        text: '$title: ',
        children: [
          TextSpan(
            text: content,
            style: TextStyle(
              fontSize: 18,
              color: Theme.of(context).colorScheme.primary,
            ),
          )
        ],
      ),
      style: const TextStyle(fontSize: 20),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        title: Text(widget.title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            FutureBuilder<Map?>(
              future: _fileContent,
              builder: (context, snapshot) {
                final bool shouldShowWidget = snapshot.data != null;

                if (!shouldShowWidget) {
                  return Visibility(visible: false, child: Container());
                }

                final timestamp =
                    DateTime.parse(snapshot.data!['timestamp']).toLocal();

                final formattedTime =
                    DateFormat('dd/MM/yyyy HH:mm:ss').format(timestamp);

                final charName = snapshot.data!['data']['mainPlayerName'];

                return Visibility(
                  visible: shouldShowWidget,
                  child: Column(
                    children: [
                      buildRichText('Data do salvamento', formattedTime),
                      buildRichText('Nome do personagem', charName),
                    ],
                  ),
                );
              },
            ),
            const Padding(padding: EdgeInsets.symmetric(vertical: 10)),
            TextButton(
              onPressed: () async {
                await createSave();
              },
              style: _buttonStyle,
              child: const Text('Salvar'),
            ),
            const Padding(padding: EdgeInsets.symmetric(vertical: 10)),
            TextButton(
              onPressed: () async {
                await createSave(withSync: true);
              },
              style: _buttonStyle,
              child: const Text('Salvar com sincronização'),
            ),
            const Padding(padding: EdgeInsets.symmetric(vertical: 10)),
            TextButton(
              onPressed: () async {
                await deleteSave();
              },
              style: _buttonStyle,
              child: const Text('Excluir salvamento'),
            ),
            const Padding(padding: EdgeInsets.symmetric(vertical: 10)),
            TextButton(
              onPressed: () async {
                final file = File(await _filePath);
                await uploadSave([file]);
              },
              style: _buttonStyle,
              child: const Text('Upload'),
            ),
            const Padding(padding: EdgeInsets.symmetric(vertical: 10)),
            TextButton(
              onPressed: () async {
                await downloadSave();
              },
              style: _buttonStyle,
              child: const Text('Baixar último'),
            ),
          ],
        ),
      ),
    );
  }
}

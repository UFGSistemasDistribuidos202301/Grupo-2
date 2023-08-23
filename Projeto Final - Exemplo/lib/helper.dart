class Helper {
  static String serialize(String saveString) {
    final jsonString = '{$saveString}';

    final modifiedString = jsonString.replaceAll("'", '"');
    final cleanedString = modifiedString.replaceAll(RegExp(r'\t|\n|\r'), '');
    final finalString = cleanedString.replaceAllMapped(
        RegExp(r'((?:\w+-)*\w+)\s*='), (match) => '"${match.group(1)}": ');

    return finalString;
  }
}

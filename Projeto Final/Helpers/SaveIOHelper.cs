using System.Text;
using System.Text.RegularExpressions;

public static class SaveIOHelper
{
	public static string Serialize(string saveString)
	{
		var builder = new StringBuilder();
		builder.Append($"{{{saveString}}}");

		builder.Replace("'", "\"");

		var jsonString = builder.ToString();
		jsonString = Regex.Replace(jsonString, @"\t|\n|\r", "");
		jsonString = Regex.Replace(jsonString, @"((?:\w+-)*\w+)\s*=", match => $"\"{match.Groups[1].Value}\": ");
		return jsonString;
	}

	public static string Deserialize(UniversalSave save)
	{
		var builder = new StringBuilder();
		builder.Append($"id = '{save.Id}',\n");
		builder.Append($"user-id = '{save.User.Id}',\n");
		builder.Append($"game-id = '{save.Game.Id}',\n");
		builder.Append($"timestamp = '{save.Timestamp.ToString("u")}',\n");
		builder.Append($"additional-content = [\n");
		foreach (var additionalContent in save.AccessedAdditionalContent)
		{
			builder.Append($"\t{{\n\t\tid = '{additionalContent.Key}',\n\t\taccessed = {additionalContent.Value.ToString().ToLower()}\n\t}}");
			if (additionalContent.Key != save.AccessedAdditionalContent.Last().Key)
			{
				builder.Append($",\n");
			}
		}
		builder.Append($"\n],\n");
		builder.Append($"data = {{\n\t");

		string data = save.Data.ToString();
		var usavString = Regex.Replace(data.Substring(1, data.Length - 2), @"""((?:\w+-)*\w+)""\s*:", match => $"{match.Groups[1].Value} = ");
		usavString = Regex.Replace(usavString, @"\s{2,}", " ");
		builder.Append(usavString);
		builder.Replace("\"", "'");

		// Melhorar tratamento dos dados
		// foreach (var data in save.Data)
		// {
		// 	var x = data;
		// }
		builder.Append($"\n}}");

		return builder.ToString();
	}

	public static string ReadUniversalFile(IFormFile file)
	{
		using var stream = file.OpenReadStream();
		using var reader = new StreamReader(file.OpenReadStream());

		return reader.ReadToEnd();
	}

	public static void WritePlatformFile(string json)
	{
		using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"D:\dev\SaveVault\Examples", "test.psav")))
		{
			outputFile.WriteLine(json);
		}
	}

	public static void WriteUniversalFile(string usav)
	{
		using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"D:\dev\SaveVault\Examples", "test.usav")))
		{
			outputFile.WriteLine(usav);
		}
	}
}
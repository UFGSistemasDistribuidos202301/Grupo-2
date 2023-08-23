using System.Text;
using System.Text.RegularExpressions;

namespace SaveVault.Helpers;

public static class SaveIOHelper
{
	public static string Serialize(string saveString)
	{
		StringBuilder builder = new();
		builder.Append($"{{{saveString}}}");

		builder.Replace("'", "\"");

		string jsonString = builder.ToString();
		jsonString = Regex.Replace(jsonString, @"\t|\n|\r", "");
		jsonString = Regex.Replace(jsonString, @"((?:\w+-)*\w+)\s*=", match => $"\"{match.Groups[1].Value}\": ");
		return jsonString;
	}

	public static string ReadFile(IFormFile file)
	{
		using Stream stream = file.OpenReadStream();
		using StreamReader reader = new(file.OpenReadStream());

		return reader.ReadToEnd();
	}

	public static string ReadFile(MemoryStream file)
	{
		return Encoding.UTF8.GetString(file.ToArray());
	}

	public static void WritePlatformFile(string psav)
	{
		using StreamWriter outputFile = new(Path.Combine(@"D:\dev\SaveVault\Examples", "test.psav"));
		outputFile.WriteLine(psav);
	}

	public static void WriteUniversalFile(string usav)
	{
		using StreamWriter outputFile = new(Path.Combine(@"D:\dev\SaveVault\Examples", "test.usav"));
		outputFile.WriteLine(usav);
	}
}

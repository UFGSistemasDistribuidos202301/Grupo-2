namespace SaveVault.Models;

public class SVFile
{
	private SVFile(string name, string extension, string content)
	{
		Name = $"{name}.{extension}";
		Type = "application/octet-stream";
		Content = System.Text.Encoding.UTF8.GetBytes(content);
	}

	public static SVFile CreatePlatformSaveFile(string name, string content) => new(name, "psav", content);
	public static SVFile CreateUniversalSaveFile(string name, string content) => new(name, "usav", content);

	public string Name { get; set; }
	public string Type { get; set; }
	public byte[] Content { get; set; }
}
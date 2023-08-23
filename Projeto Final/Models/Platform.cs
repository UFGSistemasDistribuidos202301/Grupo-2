namespace SaveVault.Models;

public enum Platform
{
	PC,
	Android,
	iOS
}

public class PlatformData
{
	private PlatformData(Platform platform)
	{
		Name = platform;
	}

	public Platform Name { get; set; }

	public static readonly PlatformData PC = new(Platform.PC)
	{
	};

	public static readonly PlatformData Android = new(Platform.Android)
	{
	};

	public static readonly PlatformData iOS = new(Platform.iOS)
	{
	};

	public static PlatformData GetPlatform(string platform) => platform switch
	{
		"PC" => PC,
		"Android" => Android,
		_ => iOS
	};
}
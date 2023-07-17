public enum Platform
{
	PC,
	Android,
	iOS
}

public class PlatformData
{
	private PlatformData() { }

	public static PlatformData PC = new PlatformData()
	{
	};

	public static PlatformData Android = new PlatformData()
	{
	};

	public static PlatformData iOS = new PlatformData()
	{
	};

	public static PlatformData GetPlatform(string platform) => platform switch
	{
		"PC" => PC,
		"Android" => Android,
		_ => iOS
	};
}
namespace DataLib;

public static class ConsoleHelper
{
	public static void Write(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
	{
		Console.ForegroundColor = foregroundColor;
		Console.BackgroundColor = backgroundColor;
		Console.WriteLine(message);
	}

	public static string? Read(ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
	{
		Console.ForegroundColor = foregroundColor;
		Console.BackgroundColor = backgroundColor;
		return Console.ReadLine();
	}
}
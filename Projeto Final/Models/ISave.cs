namespace SaveVault.Models;

public interface ISave
{
	Guid Id { get; set; }
	Game Game { get; set; }
	User User { get; set; }
	DateTime Timestamp { get; set; }
	dynamic Data { get; set; }
}
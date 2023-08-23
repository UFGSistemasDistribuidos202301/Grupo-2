namespace SaveVault.Models;

public class QueryDto
{
	public Platform TargetPlatform { get; set; }
	public Guid GameId { get; set; }
	public Guid UserId { get; set; }
	public Guid SaveId { get; set; }
}
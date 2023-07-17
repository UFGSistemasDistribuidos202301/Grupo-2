public class PlatformSave : ISave
{
	public PlatformSave(Game game, User user, PlatformData platform, DateTime timestamp, List<Guid> accessedContent, dynamic data, Guid? id = null)
	{
		Game = game;
		User = user;
		Platform = platform;
		Timestamp = timestamp;
		AccessedContent = accessedContent;
		Data = data;
		Id = id ?? Guid.NewGuid();
	}
	public Guid Id { get; set; }
	public PlatformData Platform { get; set; }
	public Game Game { get; set; }
	public User User { get; set; }
	public DateTime Timestamp { get; set; }
	public List<Guid> AccessedContent { get; set; }
	public dynamic Data { get; set; }
}
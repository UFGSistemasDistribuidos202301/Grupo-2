public class UniversalSave : ISave
{
	public UniversalSave(Game game, User user, DateTime timestamp, Dictionary<AdditionalContent, bool> accessedAdditionalContent, dynamic data, Guid? id = null)
	{
		Game = game;
		User = user;
		Timestamp = timestamp;
		AccessedAdditionalContent = accessedAdditionalContent;
		Data = data;
		Id = id ?? Guid.NewGuid();
	}
	public Guid Id { get; set; }
	public Game Game { get; set; }
	public User User { get; set; }
	public DateTime Timestamp { get; set; }
	public Dictionary<AdditionalContent, bool> AccessedAdditionalContent { get; set; }
	public dynamic Data { get; set; }
}
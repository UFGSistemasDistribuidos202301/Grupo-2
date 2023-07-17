public class Game
{
	public Guid Id { get; set; }
	public List<AdditionalContent> AdditionalContents { get; set; } = new List<AdditionalContent>();
}
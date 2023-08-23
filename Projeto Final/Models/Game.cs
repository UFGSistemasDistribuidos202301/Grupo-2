namespace SaveVault.Models;

public record Game(Guid Id)
{
	public List<AdditionalContent> AdditionalContents { get; set; } = new List<AdditionalContent>();
}
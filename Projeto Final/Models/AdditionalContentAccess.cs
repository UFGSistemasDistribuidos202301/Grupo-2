namespace SaveVault.Models;

public class AdditionalContentAccess
{
	public AdditionalContentAccess()
	{
		WasAccessed = false;
		Id = Guid.NewGuid();
	}

	public AdditionalContentAccess(AdditionalContent additionalContent, bool? wasAccessed, Guid? id = null)
	{
		AdditionalContent = additionalContent;
		WasAccessed = wasAccessed ?? false;
		Id = id ?? Guid.NewGuid();
	}

	public Guid Id { get; set; }
	public bool WasAccessed { get; set; }
	public AdditionalContent AdditionalContent { get; set; }
}
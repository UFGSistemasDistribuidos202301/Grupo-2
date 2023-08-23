using System.Text;

namespace SaveVault.Models;

public class UniversalSave : AbstractSave
{
	public UniversalSave() : base() { }

	public UniversalSave(
		Game game,
		User user,
		DateTime timestamp,
		List<AdditionalContentAccess> accessedAdditionalContent,
		dynamic data,
		Guid? id = null
	) : base(game, user, timestamp, (object)data, id)
	{
		AccessedAdditionalContent = accessedAdditionalContent;
	}
	public List<AdditionalContentAccess> AccessedAdditionalContent { get; set; }

	protected override void AppendAdditionalContent(StringBuilder builder)
	{
		foreach (AdditionalContentAccess additionalContent in AccessedAdditionalContent)
		{
			builder.Append($"\t{{\n\t\tid = '{additionalContent.AdditionalContent.Id}',\n\t\taccessed = {additionalContent.WasAccessed.ToString().ToLower()}\n\t}}");
			if (additionalContent.AdditionalContent.Id != AccessedAdditionalContent.Last().AdditionalContent.Id)
			{
				builder.Append($",\n");
			}
		}
	}
}
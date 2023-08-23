using System.Text;

namespace SaveVault.Models;

public class PlatformSave : AbstractSave
{
	public PlatformSave() : base() { }

	public PlatformSave(
		Game game,
		User user,
		PlatformData platform,
		DateTime timestamp,
		List<Guid> accessedContent,
		dynamic data,
		Guid? id = null
	) : base(game, user, timestamp, (object)data, id)
	{
		Platform = platform;
		AccessedContent = accessedContent;
	}

	public PlatformData Platform { get; set; }
	public List<Guid> AccessedContent { get; set; }

	protected override void AppendAdditionalContent(StringBuilder builder)
	{
		foreach (Guid additionalContent in AccessedContent)
		{
			builder.Append($"\t'{additionalContent}'");
			if (additionalContent != AccessedContent.Last())
			{
				builder.Append($",\n");
			}
		}
	}

	protected override void AppendPlatform(StringBuilder builder)
	{
		builder.Append($"platform = '{Platform.Name}',\n");
	}
}
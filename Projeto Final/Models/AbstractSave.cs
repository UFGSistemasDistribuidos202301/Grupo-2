using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SaveVault.Models;

public abstract class AbstractSave : ISave
{
	public AbstractSave() => Id = Guid.NewGuid();

	public AbstractSave(Game game, User user, DateTime timestamp, dynamic data, Guid? id = null)
	{
		Game = game;
		User = user;
		Timestamp = timestamp;
		Data = data;
		Id = id ?? Guid.NewGuid();
	}

	public Guid Id { get; set; }
	public Game Game { get; set; }
	public User User { get; set; }
	public DateTime Timestamp { get; set; }
	[NotMapped]
	public dynamic Data { get; set; }
	public string SerializedData
	{
		get => JsonSerializer.Serialize(Data);
		set => Data = JsonSerializer.Deserialize<dynamic>(value)!;
	}

	public string ToFileString()
	{
		StringBuilder builder = new();
		builder.Append($"id = '{Id}',\n");
		builder.Append($"user-id = '{User.Id}',\n");
		builder.Append($"game-id = '{Game.Id}',\n");
		AppendPlatform(builder);
		builder.Append($"timestamp = '{Timestamp:yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'}',\n");
		builder.Append($"additional-content = [\n");
		AppendAdditionalContent(builder);
		builder.Append($"\n],\n");
		builder.Append($"data = {{\n\t");

		string data = Data.ToString();
		string formattedData = Regex.Replace(data[1..^1], @"""((?:\w+-)*\w+)""\s*:", match => $"{match.Groups[1].Value} = ");
		formattedData = Regex.Replace(formattedData, @"\s{2,}", " ");
		builder.Append(formattedData);
		builder.Replace("\"", "'");
		builder.Append($"\n}}");

		return builder.ToString();
	}

	protected abstract void AppendAdditionalContent(StringBuilder builder);

	protected virtual void AppendPlatform(StringBuilder builder) { }
}
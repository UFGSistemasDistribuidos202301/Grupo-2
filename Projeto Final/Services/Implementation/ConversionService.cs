using System.Text.Json;
using SaveVault.Helpers;
using SaveVault.Models;

namespace SaveVault.Services.Implementation;

public class ConversionService : IConversionService
{
	public readonly IUserService _userService;
	public readonly IGameService _gameService;

	public ConversionService(IUserService userService, IGameService gameService)
	{
		_userService = userService;
		_gameService = gameService;
	}

	public UniversalSave Convert(PlatformSave save)
	{
		IEnumerable<AdditionalContentAccess> additionalContent = save.Game.AdditionalContents
			.Select(a => new AdditionalContentAccess(a, save.AccessedContent.Contains(a.Id)));

		return new UniversalSave(save.Game, save.User, save.Timestamp, additionalContent.ToList(), save.Data);
	}

	public PlatformSave Convert(UniversalSave save, Platform platform)
	{
		IEnumerable<Guid> additionalContent = save.AccessedAdditionalContent
			.Where(a => a.WasAccessed)
			.Select(a => a.AdditionalContent.Id);

		return new PlatformSave(save.Game, save.User, PlatformData.GetPlatform(platform.ToString()!), save.Timestamp, additionalContent.ToList(), save.Data, save.Id);
	}

	private async Task<T> ConvertFromString<T>(string file) where T : ISave
	{
		string jsonString = SaveIOHelper.Serialize(file);

		Dictionary<string, dynamic> jsonObject = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonString)!;

		ISave save;

		Guid? saveId = null;
		if (jsonObject.TryGetValue("id", out dynamic id))
		{
			saveId = id.GetGuid();
		}
		Guid userId = jsonObject["user-id"].GetGuid();
		Guid gameId = jsonObject["game-id"].GetGuid();
		User user = _userService.GetById(userId);
		Game game = await _gameService.GetById(gameId);
		DateTime time = jsonObject["timestamp"].GetDateTime();

		dynamic data = jsonObject["data"];

		if (typeof(T) == typeof(PlatformSave))
		{
			PlatformData platform = PlatformData.GetPlatform(jsonObject["platform"].ToString());

			List<Guid> accessedContent = new();

			foreach (dynamic content in jsonObject["accessed-content"].EnumerateArray())
			{
				accessedContent.Add(content.GetGuid());
			}

			save = new PlatformSave(game, user, platform, time, accessedContent, data, saveId);
		}
		else
		{
			List<AdditionalContentAccess> accessedContent = new();

			foreach (dynamic content in jsonObject["additional-content"].EnumerateArray())
			{
				Dictionary<string, dynamic> contentObject = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(content)!;

				Guid contentId = contentObject["id"].GetGuid();
				bool accessed = contentObject["accessed"].GetBoolean();
				AdditionalContent gameContent = new(contentId);
				AdditionalContentAccess access = new(gameContent, accessed);
				accessedContent.Add(access);
			}

			save = new UniversalSave(game, user, time, accessedContent, data, saveId);
		}

		return (T)System.Convert.ChangeType(save, typeof(T));
	}

	public async Task<T> ConvertFromFile<T>(IFormFile file) where T : ISave
	{
		string saveString = SaveIOHelper.ReadFile(file);
		return await ConvertFromString<T>(saveString);
	}

	public async Task<T> ConvertFromFile<T>(MemoryStream file) where T : ISave
	{
		string saveString = SaveIOHelper.ReadFile(file);
		return await ConvertFromString<T>(saveString);
	}
}
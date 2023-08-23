using SaveVault.Models;
using SaveVault.Repositories;

namespace SaveVault.Services.Implementation;

public class DownloadService : IDownloadService
{
	private readonly IDownloadRepository _downloadRepository;
	private readonly IConversionService _conversionService;

	public DownloadService(IDownloadRepository downloadRepository, IConversionService conversionService)
	{
		_downloadRepository = downloadRepository;
		_conversionService = conversionService;
	}

	public async Task<IEnumerable<ISave>> DownloadAllSaves(Game game, User user)
	{
		IEnumerable<MemoryStream> files = await _downloadRepository.DownloadAll(game, user);

		List<ISave> result = new();
		foreach (MemoryStream file in files)
		{
			result.Add(await _conversionService.ConvertFromFile<UniversalSave>(file));
		}
		return result;
	}

	public async Task<ISave> DownloadById(Guid saveId)
	{
		MemoryStream file = await _downloadRepository.DownloadById(saveId);
		UniversalSave save = await _conversionService.ConvertFromFile<UniversalSave>(file);
		return save;
	}

	public async Task<ISave> DownloadLatest(Game game, User user)
	{
		MemoryStream file = await _downloadRepository.DownloadLatest(game, user);
		UniversalSave save = await _conversionService.ConvertFromFile<UniversalSave>(file);
		return save;
	}

	public SVFile CreatePlatformFile(string fileName, string content) => SVFile.CreatePlatformSaveFile(fileName, content);
}
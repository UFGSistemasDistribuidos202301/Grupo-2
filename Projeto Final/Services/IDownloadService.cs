using SaveVault.Models;

namespace SaveVault.Services;

public interface IDownloadService
{
	Task<IEnumerable<ISave>> DownloadAllSaves(Game game, User user);
	Task<ISave> DownloadById(Guid saveId);
	Task<ISave> DownloadLatest(Game game, User user);
	SVFile CreatePlatformFile(string fileName, string saveString);
}
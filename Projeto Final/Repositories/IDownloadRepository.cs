using SaveVault.Models;

namespace SaveVault.Repositories;

public interface IDownloadRepository
{
	Task<MemoryStream> DownloadLatest(Game game, User user);
	Task<IEnumerable<MemoryStream>> DownloadAll(Game game, User user);
	Task<MemoryStream> DownloadById(Guid saveId);
}
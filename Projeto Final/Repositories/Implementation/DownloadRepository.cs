using Google.Cloud.Firestore;
using SaveVault.Models;

namespace SaveVault.Repositories.Implementation;

public class DownloadRepository : IDownloadRepository
{
	private readonly IFirebaseRepository _firebaseRepository;

	public DownloadRepository(IFirebaseRepository firebaseRepository)
	{
		_firebaseRepository = firebaseRepository;
	}

	public async Task<MemoryStream> DownloadLatest(Game game, User user)
	{
		QuerySnapshot saveSnapshot = await _firebaseRepository.GetSaves(user, game)
			.OrderByDescending("timestamp")
			.Limit(1)
			.GetSnapshotAsync();

		DocumentSnapshot save = saveSnapshot[0];

		return await _firebaseRepository.Download(save.Id.ToString());
	}

	public async Task<IEnumerable<MemoryStream>> DownloadAll(Game game, User user)
	{
		QuerySnapshot saveSnapshot = await _firebaseRepository.GetSaves(user, game)
			.OrderByDescending("timestamp")
			.GetSnapshotAsync();

		List<MemoryStream> result = new();

		foreach (var save in saveSnapshot)
		{
			result.Add(await _firebaseRepository.Download(save.Id.ToString()));
		}

		return result;
	}

	public async Task<MemoryStream> DownloadById(Guid saveId)
	{
		return await _firebaseRepository.Download(saveId.ToString());
	}
}
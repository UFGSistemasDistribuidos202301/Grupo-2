using Google.Cloud.Firestore;
using SaveVault.Models;

namespace SaveVault.Repositories.Implementation;

public class GameRepository : IGameRepository
{
	private readonly IFirebaseRepository _firebaseRepository;

	public GameRepository(IFirebaseRepository firebaseRepository)
	{
		_firebaseRepository = firebaseRepository;
	}

	public async Task<Game> GetById(Guid gameId)
	{
		string id = _firebaseRepository.GetGames().Document(gameId.ToString()).Id;
		Game game = new(new Guid(id))
		{
			AdditionalContents = new List<AdditionalContent>()
		};

		CollectionReference dlcs = _firebaseRepository.GetAdditionalContent(game);
		QuerySnapshot querySnapshot = await dlcs.GetSnapshotAsync();

		game.AdditionalContents = querySnapshot.Documents
			.Select(s => new AdditionalContent(new Guid(s.Id)))
			.ToList();

		return game;
	}
}

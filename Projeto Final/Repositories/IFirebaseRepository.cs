using Google.Cloud.Firestore;
using SaveVault.Models;

namespace SaveVault.Repositories;

public interface IFirebaseRepository
{
	CollectionReference GetUsers();
	CollectionReference GetGames();
	CollectionReference GetAdditionalContent(Game game);
	CollectionReference GetUserGames(User user);
	CollectionReference GetSaves(User user, Game game);
	Task<MemoryStream> Download(string fileId);
	Task Upload(UniversalSave saveFile);
}
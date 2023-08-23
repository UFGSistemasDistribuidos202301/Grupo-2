using System.Text;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using SaveVault.Models;

namespace SaveVault.Repositories.Implementation;

public class FirebaseRepository : IFirebaseRepository
{
	private readonly FirestoreDb _db;
	private readonly StorageClient _storage;

	private readonly string BucketName = "save-vault.appspot.com";

	public FirebaseRepository(FirestoreDb db, StorageClient storage)
	{
		_db = db;
		_storage = storage;
	}

	public CollectionReference GetUsers() => _db.Collection("users");

	public CollectionReference GetGames() => _db.Collection("games");

	public CollectionReference GetAdditionalContent(Game game)
	{
		CollectionReference gamesCollection = _db.Collection("games");
		DocumentReference gameDocument = gamesCollection.Document(game.Id.ToString());

		return gameDocument.Collection("dlcs");
	}

	public CollectionReference GetUserGames(User user)
	{
		CollectionReference usersCollection = _db.Collection("users");
		DocumentReference userDocument = usersCollection.Document(user.Id.ToString());

		return userDocument.Collection("games");
	}

	public CollectionReference GetSaves(User user, Game game)
	{
		CollectionReference gameCollection = GetUserGames(user);
		DocumentReference gameDocument = gameCollection.Document(game.Id.ToString());

		return gameDocument.Collection("saves");
	}

	public async Task<MemoryStream> Download(string fileId)
	{
		using MemoryStream memoryStream = new();
		await _storage.DownloadObjectAsync(BucketName, $"{fileId}.usav", memoryStream);
		return memoryStream;
	}

	public async Task Upload(UniversalSave saveFile)
	{
		string saveString = saveFile.ToFileString();
		using MemoryStream stream = new(Encoding.UTF8.GetBytes(saveString));

		await _db.RunTransactionAsync(async transaction =>
		{
			CollectionReference saveCollection = GetSaves(saveFile.User, saveFile.Game);
			DocumentReference saveDocument = saveCollection.Document(saveFile.Id.ToString());

			Dictionary<string, object> saveObject = new()
			{
				{"timestamp", saveFile.Timestamp}
			};

			transaction.Set(saveDocument, saveObject);

			await _storage.UploadObjectAsync(BucketName, $"{saveFile.Id}.usav", "application/octet-stream", stream);
		});
	}
}
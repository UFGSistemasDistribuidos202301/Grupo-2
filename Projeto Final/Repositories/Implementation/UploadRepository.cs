using SaveVault.Models;

namespace SaveVault.Repositories.Implementation;

public class UploadRepository : IUploadRepository
{
	private readonly IFirebaseRepository _firebaseRepository;

	public UploadRepository(IFirebaseRepository firebaseRepository)
	{
		_firebaseRepository = firebaseRepository;
	}

	public async Task Upload(UniversalSave save) => await _firebaseRepository.Upload(save);
}
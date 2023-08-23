using SaveVault.Models;

namespace SaveVault.Repositories.Implementation;

public class UserRepository : IUserRepository
{
	private readonly IFirebaseRepository _firebaseRepository;

	public UserRepository(IFirebaseRepository firebaseRepository)
	{
		_firebaseRepository = firebaseRepository;
	}

	public User GetById(Guid userId)
	{
		string id = _firebaseRepository.GetUsers().Document(userId.ToString()).Id;
		return new User(new Guid(id));
	}
}
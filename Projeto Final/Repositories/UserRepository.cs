public class UserRepository : IUserRepository
{
	public User GetById(Guid userId)
	{
		//Remover mock depois
		return new User()
		{
			Id = userId,
		};
	}
}
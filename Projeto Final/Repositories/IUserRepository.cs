using SaveVault.Models;

namespace SaveVault.Repositories;

public interface IUserRepository
{
	User GetById(Guid userId);
}
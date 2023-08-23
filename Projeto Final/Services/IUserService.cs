using SaveVault.Models;

namespace SaveVault.Services;

public interface IUserService
{
	User GetById(Guid userId);
}
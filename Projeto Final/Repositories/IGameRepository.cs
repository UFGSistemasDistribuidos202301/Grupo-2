using SaveVault.Models;

namespace SaveVault.Repositories;

public interface IGameRepository
{
	Task<Game> GetById(Guid gameId);
}
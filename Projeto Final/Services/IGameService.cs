using SaveVault.Models;

namespace SaveVault.Services;

public interface IGameService
{
	Task<Game> GetById(Guid gameId);
}
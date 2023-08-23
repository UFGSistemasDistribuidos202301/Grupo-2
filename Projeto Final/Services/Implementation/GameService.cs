using SaveVault.Models;
using SaveVault.Repositories;

namespace SaveVault.Services.Implementation;

public class GameService : IGameService
{
	private readonly IGameRepository _gameRepository;

	public GameService(IGameRepository gameRepository)
	{
		_gameRepository = gameRepository;
	}

	public async Task<Game> GetById(Guid gameId)
	{
		return await _gameRepository.GetById(gameId);
	}
}
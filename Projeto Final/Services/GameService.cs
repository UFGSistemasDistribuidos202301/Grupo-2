public class GameService : IGameService
{
	private readonly IGameRepository _gameRepository;

	public GameService(IGameRepository gameRepository)
	{
		_gameRepository = gameRepository;
	}

	public Game GetById(Guid gameId)
	{
		return _gameRepository.GetById(gameId);
	}
}
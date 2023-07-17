public class GameRepository : IGameRepository
{
	public Game GetById(Guid gameId)
	{
		//Remover mock depois
		return new Game()
		{
			Id = gameId,
			AdditionalContents = new List<AdditionalContent>()
			{
				new AdditionalContent() { Id = new Guid("23815979-e9c5-45df-9aa3-d8acd219996b") },
				new AdditionalContent() { Id = new Guid("d1d5de9a-72b7-4ff8-9cf0-dbc743f39f33") },
			}
		};
	}
}
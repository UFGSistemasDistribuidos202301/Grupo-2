public class DownloadService : IDownloadService
{
	public IEnumerable<ISave> GetAllSaves(Game game, User user)
	{
		throw new NotImplementedException();
	}

	public ISave GetById(Guid SaveId, User user)
	{
		throw new NotImplementedException();
	}

	public ISave GetLatest(Game game, User user)
	{
		throw new NotImplementedException();
	}
}
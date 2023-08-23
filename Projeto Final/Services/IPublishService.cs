using SaveVault.Models;

namespace SaveVault.Services;

public interface IPublishService
{
	void PublishMessage(ISave save);
}
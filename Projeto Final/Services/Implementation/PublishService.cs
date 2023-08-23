using System.Text;
using RabbitMQ.Client;
using SaveVault.Models;

namespace SaveVault.Services.Implementation;

public class PublishService : IPublishService
{
	private readonly IModel _channel;

	private readonly string Exchange = "SaveVault";

	public PublishService(IModel channel)
	{
		_channel = channel;
	}

	public void PublishMessage(ISave save)
	{
		string routingKey = $"{save.User.Id}.{save.Game.Id}";
		string message = save.Id.ToString();
		byte[] body = Encoding.UTF8.GetBytes(message);
		_channel.BasicPublish(Exchange, routingKey, null, body);
		Console.WriteLine($"Message sent: {message}");
	}
}
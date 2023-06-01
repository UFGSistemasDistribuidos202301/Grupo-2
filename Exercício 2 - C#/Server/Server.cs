using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DataLib;

namespace Server;

class SocketServer
{
	private static readonly int NUMBER_OF_CONNECTIONS = 10;

	public static void Execute()
	{
		Console.Clear();
		Socket listener = SocketHelper.GetSocket();

		try
		{
			listener.Bind(SocketHelper.Endpoint);
			listener.Listen(NUMBER_OF_CONNECTIONS);

			ConsoleHelper.Write($"Aguardando conexão em {SocketHelper.Endpoint.Address}:{SocketHelper.Endpoint.Port}...", ConsoleColor.Blue);

			Socket clientSocket = listener.Accept();

			int receivedMessageSize = 0;
			bool shouldRunServer = true;
			while (shouldRunServer)
			{
				byte[] receivedMessage = new byte[1024];
				receivedMessageSize = clientSocket.Receive(receivedMessage);
				if (receivedMessageSize > 0)
				{
					ConsoleHelper.Write("Mensagem recebida!", ConsoleColor.Green);
					string data = Encoding.UTF8.GetString(receivedMessage, 0, receivedMessageSize);

					if (data == "STOP")
					{
						shouldRunServer = false;
					}
					else
					{
						Person person = JsonSerializer.Deserialize<Person>(data)!;

						string messageText = person.ReachedAdulthood()
							? $"{person.Name} já atingiu a maioridade."
							: $"Falta(m) {person.CalculateRemainingYearsUntilAdulthood()} ano(s) para {person.Name} atingir a maioridade.";

						byte[] messageToSend = Encoding.UTF8.GetBytes(messageText);
						clientSocket.Send(messageToSend);
					}
				}
			}

			ConsoleHelper.Write($"Encerrando conexão...", ConsoleColor.Blue);

			clientSocket.Shutdown(SocketShutdown.Both);
			clientSocket.Close();
		}
		catch (Exception e)
		{
			ConsoleHelper.Write(e.ToString(), ConsoleColor.Red);
		}
	}
}

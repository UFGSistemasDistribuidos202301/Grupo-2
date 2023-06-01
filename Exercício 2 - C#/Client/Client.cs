using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using DataLib;

namespace Client;

class SocketClient
{
	public static void Execute()
	{
		Console.Clear();
		Socket sender = SocketHelper.GetSocket();

		try
		{
			sender.Connect(SocketHelper.Endpoint);

			ConsoleHelper.Write($"Socket conectado em {SocketHelper.Endpoint.Address}:{SocketHelper.Endpoint.Port}", ConsoleColor.Green);
			ConsoleHelper.Write($"CALCULADORA DE MAIORIDADE", ConsoleColor.White, ConsoleColor.Magenta);

			bool shouldRunClient = true;
			while (shouldRunClient)
			{
				ConsoleHelper.Write("\nPressione [q/Q] para sair ou qualquer outra tecla para continuar", ConsoleColor.Magenta);
				shouldRunClient = Console.ReadKey(true).Key != ConsoleKey.Q;

				if (shouldRunClient)
				{
					Person person = ReceiveInput();
					byte[] messageToSend = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(person));
					sender.Send(messageToSend);

					byte[] receivedMessage = new byte[1024];
					int receivedMessageSize = sender.Receive(receivedMessage);
					string receivedText = Encoding.UTF8.GetString(receivedMessage, 0, receivedMessageSize);

					ConsoleHelper.Write($"Messagem recebida: \"{receivedText}\"", ConsoleColor.Green);
				}
			}

			byte[] endMessage = Encoding.UTF8.GetBytes("STOP");
			sender.Send(endMessage);

			ConsoleHelper.Write($"Encerrando conexão...", ConsoleColor.Blue);

			sender.Shutdown(SocketShutdown.Both);
			sender.Close();
		}
		catch (Exception e)
		{
			ConsoleHelper.Write(e.ToString(), ConsoleColor.Red);
		}
	}

	private static Person ReceiveInput()
	{
		ConsoleHelper.Write("Digite o nome da pessoa:", ConsoleColor.Blue);
		string name = ConsoleHelper.Read()?.Trim() ?? string.Empty;

		bool invalidAge = false;
		int age = 0;
		do
		{
			if (invalidAge)
			{
				ConsoleHelper.Write("Idade inválida", ConsoleColor.Red);
			}

			ConsoleHelper.Write("Digite a idade da pessoa:", ConsoleColor.Blue);
			invalidAge = !int.TryParse(ConsoleHelper.Read(), out age) || age < 0;
		}
		while (invalidAge);

		bool invalidSex = false;
		Sex sex = Sex.Feminine;
		do
		{
			if (invalidSex)
			{
				ConsoleHelper.Write("Sexo inválido", ConsoleColor.Red);
			}

			ConsoleHelper.Write("Digite o sexo da pessoa [m/M] - Masculino [f/F] - Feminino:", ConsoleColor.Blue);
			string option = ConsoleHelper.Read()!.Trim().ToLower();

			invalidSex = option != "m" && option != "f";
			if (!invalidSex)
			{
				sex = option == "m" ? Sex.Masculine : Sex.Feminine;
			}
		}
		while (invalidSex);

		return new Person()
		{
			Name = name,
			Age = age,
			Sex = sex
		};
	}
}


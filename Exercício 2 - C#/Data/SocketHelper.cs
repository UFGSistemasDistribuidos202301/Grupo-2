using System.Net;
using System.Net.Sockets;

namespace DataLib;

public static class SocketHelper
{
	private static IPHostEntry Host { get; set; }
	private static IPAddress Address { get; set; }
	public static IPEndPoint Endpoint { get; set; }

	static SocketHelper()
	{
		Host = Dns.GetHostEntry(Dns.GetHostName());
		Address = Host.AddressList.Last();
		Endpoint = new IPEndPoint(Address, 2905);
	}

	public static Socket GetSocket() => new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
}
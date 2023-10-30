using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Example
{
	public class Server : WebSocketBehavior
	{
		protected override void OnMessage(MessageEventArgs e)
		{
			var msg = e.Data;
			Send(msg);
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			var wssv = new WebSocketServer(System.Net.IPAddress.Any, 6000);
			wssv.AddWebSocketService<Server>("/Server");
			wssv.Start();
			Console.ReadKey(true);
			wssv.Stop();
		}
	}

}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KanbanService
{
	public class Server : IDisposable
	{
		TcpListener server = null;
		Action<Stream> onConnect;

		public Server(ServerSettings settings, Action<Stream> onConnect)
		{
			this.onConnect = onConnect;
			IPAddress localAddr = IPAddress.Parse(settings.IPAddress);
			server = new TcpListener(localAddr, settings.Port);
			server.Start();
			var t = new Thread(delegate ()
			{
				StartListener();
			});
			t.Start();			
		}

		public void StartListener()
		{
			try
			{
				while (!disposedValue)
				{
					// Waiting for a connection...
					TcpClient client = server.AcceptTcpClient();
					// Connected!
					Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
					t.Start(client);
				}
			}
			catch (SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
				server.Stop();
			}
		}

		public void HandleDevice(Object obj)
		{
			TcpClient client = (TcpClient)obj;
			var stream = client.GetStream();
			onConnect(stream);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					server.Stop();
					onConnect = null;
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Server()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}

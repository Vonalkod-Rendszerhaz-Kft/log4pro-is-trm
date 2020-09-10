using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Vrh.ApplicationContainer.Control.Contract;
using Vrh.ApplicationContainer.Core;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using Log4Pro.IS.TRM.EventHubContract;
using Vrh.EventHub.Core;
using Vrh.EventHub.Protocols.RedisPubSub;

namespace KanbanService
{

	public class KanbanServicePlugin : PluginAncestor
	{
		private static IConfiguration _configuration;

		/// <summary>
		/// Constructor
		/// </summary>
		private KanbanServicePlugin()
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location))
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			_configuration = builder.Build();			
			
			EndLoad();
		}

		/// <summary>
		/// Static Factory (Ha nincs megadva, akkor egy egy paraméteres konstruktort kell implementálni, amely konstruktor paraméterben fogja megkapni a )
		/// </summary>
		/// <param name="instanceDefinition">A példány definiciója</param>
		/// <param name="instanceData">A példánynak átadott adat(ok)</param>
		/// <returns></returns>
		public static KanbanServicePlugin KanbanServicePluginFactory(InstanceDefinition instanceDefinition, Object instanceData)
		{
			var instance = new KanbanServicePlugin();
			instance._myData = instanceDefinition;
			return instance;
		}

		/// <summary>
		/// IPlugin.Start
		/// </summary>
		public override void Start()
		{
			if (MyStatus == PluginState.Starting || MyStatus == PluginState.Running)
			{
				return;
			}
			BeginStart();
			try
			{
				//Implement Start logic here

				var serverSettings = new ServerSettings();
				var readingSettings = new ReadingSettings();


				_configuration.Bind("ReadingSettings", readingSettings);
				_configuration.Bind("ServerSettings", serverSettings);

				_kanbanService = new KanbanService(readingSettings);
				_socketServer = new Server(serverSettings, _kanbanService.StartService);
				//var t = new Thread(delegate ()
				//{
				//	_socketServer = new Server(serverSettings, _kanbanService.StartService);
				//});
				//t.Start();

				base.Start();
			}
			catch (Exception ex)
			{
				SetErrorState(ex);
			}
		}

		/// <summary>
		/// IPlugin.Stop
		/// </summary>
		public override void Stop()
		{
			if (MyStatus == PluginState.Stopping || MyStatus == PluginState.Loaded)
			{
				return;
			}
			BeginStop();
			try
			{
				// Implement stop logic here
				_kanbanService?.Dispose();
				_socketServer?.Dispose();
				base.Stop();
			}
			catch (Exception ex)
			{
				SetErrorState(ex);
			}
		}

		private KanbanService _kanbanService = null;
		private Server _socketServer = null;

		#region IDisposable Support
		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					try
					{
						BeginDispose();
						// TODO: dispose managed state (managed objects).

						Stop();
					}
					finally
					{
						base.Dispose(disposing);
					}
				}
				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TestPlugin() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public override void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}


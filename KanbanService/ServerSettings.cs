using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanService
{
	public class ServerSettings
	{
		public string IPAddress { get; set; }
		public int Port { get; set; }
		public int SendTimeout { get; set; }
		public int ReceiveTimeout { get; set; }
	}
}

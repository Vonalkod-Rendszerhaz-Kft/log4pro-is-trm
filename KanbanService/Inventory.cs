using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanService
{
	public enum ResponseType { OK, NOK }

	public class Inventory
	{
		public ResponseType ResponseType { get; set; }
		public string[] EpcIds { get; set; } = new string[8];

		public Inventory(string response)
		{
			if (response[15] == '0' && response[16] == '0')
			{
				ResponseType = ResponseType.OK;
				GetEpcId(response, int.Parse(response[19].ToString()));
			}
			else
			{
				ResponseType = ResponseType.NOK;
			}
		}

		private void GetEpcId(string response, int antennas)
		{
			//Az olvasott antennák
			string[] initSelect = new string[antennas + 2];

			//Antenna adatok
			for (int i = 1; i < initSelect.Length - 1; i++)
			{
				initSelect[i] = response.Substring(21 + (i - 1) * 60, 59);
			}

			for (int i = 0; i < antennas; i++)
			{
				var antennaNumber = int.Parse(initSelect[i + 1][40].ToString()) - 1;
				string epcId = string.Empty;

				for (int k = 12; k <= 34; k++)
				{
					epcId += initSelect[i + 1][k].ToString();
				}
				EpcIds[antennaNumber] = epcId;
			}
		}
	}
}

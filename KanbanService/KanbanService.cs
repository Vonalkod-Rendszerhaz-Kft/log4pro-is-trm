using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace KanbanService
{
	public class KanbanService
	{
		private Inventory current;
		private Timer timer;
		private ReadingSettings readingSettings;

		public KanbanService(ReadingSettings readingSettings)
		{
			this.readingSettings = readingSettings;
		}

		public void StartService(Stream stream)
		{
			StartTimer(stream);
		}

		public void StartTimer(Stream stream)
		{
			var comm = new FeigCommunication(readingSettings, stream);
			timer = new Timer(
				(e) => OnInventory(stream, comm),
				null,
				TimeSpan.Zero,
				TimeSpan.FromMilliseconds(readingSettings.Interval));
		}

		public void OnInventory(Stream stream, FeigCommunication comm)
		{
			try
			{
				Inventory previous = current;
				current = comm.Inventory();

				if (previous != null)
				{
					GetDifference(previous.EpcIds, current.EpcIds);
				}
			}
			catch (Exception)
			{
				timer?.Change(Timeout.Infinite, 0);
				timer?.Dispose();
			}
		}

		private void GetDifference(string[] previous, string[] current)
		{
			for (int i = 0; i < previous.Length; i++)
			{
				if (previous[i] != current[i])
				{
					if (previous[i] == null && current[i] != null)
					{
						// felkerült egy új 
					}
					else if (previous[i] != null && current[i] == null)
					{
						// lekerült egy doboz
					}
					else
					{
						// egy másik doboz került a helyére
					}
				}
			}
		}
	}
}

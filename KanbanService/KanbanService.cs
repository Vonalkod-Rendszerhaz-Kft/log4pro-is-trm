using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Log4Pro.IS.TRM.EventHubContract;
using Vrh.EventHub.Core;
using Vrh.EventHub.Protocols.RedisPubSub;

namespace KanbanService
{
	public class KanbanService
	{
		private Inventory current;
		private Timer timer;
		private ReadingSettings readingSettings;

		/// <summary>
		/// Munkahely instance neve TODO: itt iplmentáld, ha később az konfigurálható kell legyen, vagy valami logika lapján kell képződjön!!!
		/// </summary>
		private string InstanceName { get => "demo"; }

		/// <summary>
		/// Eventhub csatorna neve
		/// </summary>
		private string EventHubChannelName { get => $"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.KanbanModule.MODULE_PREFIX}:{InstanceName}"; }

		private List<TrackingContract.KanbanModule.LocationMap> _locationMaps;

		public KanbanService(ReadingSettings readingSettings)
		{
			this.readingSettings = readingSettings;
			_locationMaps = new List<TrackingContract.KanbanModule.LocationMap>();
			for (int i = 1; i <= 8; i++) {
				TrackingContract.KanbanModule.LocationMap locationMap = new TrackingContract.KanbanModule.LocationMap();
				locationMap.Location = i.ToString();
				locationMap.Status = TrackingContract.KanbanModule.KanbanLocationStatus.Free;
				_locationMaps.Add(locationMap);
			}
			TrackingContract.KanbanModule.ChangeEvent changeEvent = new TrackingContract.KanbanModule.ChangeEvent
			{
				LocationMap = _locationMaps,
			};
			EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.ChangeEvent>(EventHubChannelName, changeEvent);
			EventHubCore.RegisterHandler<RedisPubSubChannel,
					TrackingContract.KanbanModule.ReservationRequest, TrackingContract.KanbanModule.ReservationResponse>(EventHubChannelName, LocationReservation);
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
				else 
				{
					FirstLoad(current.EpcIds);
				}
			}
			catch (Exception)
			{
				timer?.Change(Timeout.Infinite, 0);
				timer?.Dispose();
			}
		}

		private void FirstLoad(string[] current) 
		{
			for (int i = 0; i < current.Length; i++)
			{
				LocationMapUpdate((i + 1), current[i]);
			}
		}

		private void GetDifference(string[] previous, string[] current)
		{
			for (int i = 0; i < previous.Length; i++)
			{
				if (previous[i] != current[i])
				{
					LocationMapUpdate((i + 1), current[i]);

					if (previous[i] == null && current[i] != null)
					{
						// felkerült egy új
						TrackingContract.KanbanModule.SuccessStoreIn successStoreIn = new TrackingContract.KanbanModule.SuccessStoreIn();
						successStoreIn.PackageUnitId = current[i];
						EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.SuccessStoreIn>(EventHubChannelName, successStoreIn);
					}
					else if (previous[i] != null && current[i] == null)
					{
						// lekerült egy doboz
						TrackingContract.KanbanModule.SuccessStoreOut successStoreOut = new TrackingContract.KanbanModule.SuccessStoreOut();
						successStoreOut.PackageUnitId = current[i];
						EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.SuccessStoreOut>(EventHubChannelName, successStoreOut);
					}
					else 
					{
						// egy másik doboz került a helyére
					}
					TrackingContract.KanbanModule.ChangeEvent changeEvent = new TrackingContract.KanbanModule.ChangeEvent
					{
						LocationMap = _locationMaps,
					};
					EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.ChangeEvent>(EventHubChannelName, changeEvent);					
				}
			}
		}

		private void LocationMapUpdate(int location, string packageUnitId)
		{
			var locationMap = _locationMaps.FirstOrDefault(x => x.Location == location.ToString());

			if (locationMap != null)
			{
				if (locationMap.Status != TrackingContract.KanbanModule.KanbanLocationStatus.Inactive)
				{
					//Inaktív tárolóhely
					return;
				}

				locationMap.LoadedPackageUnitId = packageUnitId;

				if (locationMap.Status == TrackingContract.KanbanModule.KanbanLocationStatus.Reserved &&
					locationMap.ExpectedPackagingUnit != packageUnitId)
				{
					locationMap.IsError = true;
				}

				if (!String.IsNullOrEmpty(locationMap.LoadedPackageUnitId))
				{
					locationMap.Status = TrackingContract.KanbanModule.KanbanLocationStatus.Loaded;
				}
				else
				{
					locationMap.Status = TrackingContract.KanbanModule.KanbanLocationStatus.Free;
				}
			}
			else 
			{
				// Nem létező lokáció
				return;
			}

		}

		private Response<TrackingContract.KanbanModule.ReservationResponse> LocationReservation(Request<TrackingContract.KanbanModule.ReservationRequest, TrackingContract.KanbanModule.ReservationResponse> message)
		{
			Response<TrackingContract.KanbanModule.ReservationResponse> reservationResponseMessage = message.MyResponse;
			try
			{
				var locationMap = _locationMaps.FirstOrDefault(x => x.Status == TrackingContract.KanbanModule.KanbanLocationStatus.Free);
				if (locationMap != null)
				{
					locationMap.ExpectedPackagingUnit = message.RequestContent.PackagingUnitId;
					locationMap.Status = TrackingContract.KanbanModule.KanbanLocationStatus.Reserved;
					reservationResponseMessage.ResponseContent = new TrackingContract.KanbanModule.ReservationResponse()
					{
						ReservedLocation = locationMap.Location,
					};
					TrackingContract.KanbanModule.ChangeEvent changeEvent = new TrackingContract.KanbanModule.ChangeEvent
					{
						LocationMap = _locationMaps,
					};
					EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.ChangeEvent>(EventHubChannelName, changeEvent);
				}
				else
				{
					throw new ApplicationException("A foglalás sikertelen, nincs szabad lokáció!");
				}
			}
			catch (Exception ex)
			{
				reservationResponseMessage.Exception = ex;
			}

			return reservationResponseMessage;
		}
	}
}

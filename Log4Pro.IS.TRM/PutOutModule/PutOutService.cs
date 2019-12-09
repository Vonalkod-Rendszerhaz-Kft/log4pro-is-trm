using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log4Pro.IS.TRM.EventHubContract;
using Vrh.EventHub.Core;
using Vrh.EventHub.Protocols.RedisPubSub;
using Log4Pro.IS.TRM.DAL;
using System.Xml.Linq;

namespace Log4Pro.IS.TRM.PutOutModule
{
    internal class PutOutService : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PutOutService()
        {
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                TrackingContract.PutOutModule.PutOutRequest,
                TrackingContract.Response>(EventHubChannelName, PutOut);
        }

        /// <summary>
        /// Betárolás a raktárba
        /// </summary>
        /// <param name="request">kérés</param>
        /// <returns>válasz</returns>
        private Response<TrackingContract.Response> PutOut(Request<TrackingContract.PutOutModule.PutOutRequest, TrackingContract.Response> request)
        {
            var response = request.MyResponse;
            try
            {
                using (var dbc = new ISTRMContext())
                {
                    var storedPackagingUnit = dbc.PackagingUnits.FirstOrDefault(x => x.PackageUnitId == request.RequestContent.PackagingUnitId
                                                                                    && x.Active
                                                                                    && x.PackagingUnitStatus == PackagingUnitStatus.Created.ToString());
                    if (storedPackagingUnit == null)
                    {
                        throw new Exception($"This packaging unit is not in store: {request.RequestContent.PackagingUnitId}");
                    }
                    else
                    {
                        var responseFromKanbanSubSystem =
                            EventHubCore.Call<RedisPubSubChannel,
                                                TrackingContract.KanbanModule.ReservationRequest,
                                                TrackingContract.KanbanModule.ReservationResponse>(
                                            EventHubChannelName,
                                            new TrackingContract.KanbanModule.ReservationRequest()
                                            {
                                                PackagingUnitId = storedPackagingUnit.PackageUnitId,
                                            }, new TimeSpan(0, 0, 2));
                        var putedOutPackagingUnit = dbc.PutOut(storedPackagingUnit);
                        var xml = GetMonitorData(putedOutPackagingUnit, responseFromKanbanSubSystem.ReservedLocation);
                        dbc.AddMonitorData(WorkstationType.Putout, InstanceName, xml);
                        response.ResponseContent = new TrackingContract.Response();
                    }
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                try
                {
                    var xml = MonitorDataContentHelper.GetMonitorDataForErrors(ex);
                    using (var dbc = new ISTRMContext())
                    {
                        dbc.AddMonitorData(WorkstationType.Putout, InstanceName, xml);
                    }
                }
                catch { }
            }
            return response;
        }

        /// <summary>
        /// Munkahely instance neve TODO: itt iplmentáld, ha később az konfigurálható kell legyen, vagy valami logika lapján kell képződjön!!!
        /// </summary>
        private string InstanceName { get => "demo"; }

        /// <summary>
        /// Eventhub csatorna neve
        /// </summary>
        private string EventHubChannelName { get => $"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.PutOutModule.MODULE_PREFIX}:{InstanceName}"; }

        /// <summary>
        /// Visszadja a Monitorrekordba irandó adatokat
        /// </summary>
        /// <param name="putedOutPackagingUnit"></param>
        /// <returns></returns>
        private XElement GetMonitorData(PackagingUnit putedOutPackagingUnit, string kanbanTargetLocation)
        {
            var monitorDataList = new Dictionary<string, string>()
            {
                { "PackagingUnitId", putedOutPackagingUnit.PackageUnitId },
                { "PartNumber", putedOutPackagingUnit.Part.PartNumber },
                { "PartDescription", putedOutPackagingUnit.Part.Description },
                { "Qty", putedOutPackagingUnit.Quantity.ToString() },
                { "KanbanTargetLocation", kanbanTargetLocation }
            };
            return MonitorDataContentHelper.CreateContent(monitorDataList);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    EventHubCore.DropHandler<RedisPubSubChannel,
                                                    TrackingContract.PutOutModule.PutOutRequest,
                                                    TrackingContract.Response>(EventHubChannelName, PutOut);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PutOutService()
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

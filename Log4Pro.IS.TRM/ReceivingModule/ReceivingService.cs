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

namespace Log4Pro.IS.TRM.ReceivingModule
{
    /// <summary>
    /// Betárolás modul implementáció
    /// </summary>
    internal class ReceivingService : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReceivingService()
        {
            EventHubCore.RegisterHandler<RedisPubSubChannel, 
                TrackingContract.ReceivingModule.ReceiveRequest, 
                TrackingContract.ReceivingModule.ReceiveResponse>(EventHubChannelName, Receive);
        }

        /// <summary>
        /// Betárolás a raktárba
        /// </summary>
        /// <param name="request">kérés</param>
        /// <returns>válasz</returns>
        private Response<TrackingContract.ReceivingModule.ReceiveResponse> Receive(Request<TrackingContract.ReceivingModule.ReceiveRequest, TrackingContract.ReceivingModule.ReceiveResponse> request)
        {
            var response = request.MyResponse;
            try
            {
                using (var dbc = new ISTRMContext())
                {
                    var takedinShippingUnit = dbc.ShippingUnits.FirstOrDefault(x => x.ShippingUnitId == request.RequestContent.ShippingUnitId 
                                                                                    && x.Active 
                                                                                    && x.Status == ShippingUnitStatus.TakedIn);
                    if (takedinShippingUnit == null)
                    {
                        throw new Exception($"This shipping unit is not taked in: {request.RequestContent.ShippingUnitId}");
                    }
                    else
                    {
                        string location = GetStoreLocation(takedinShippingUnit);
                        var receivedShippingUnit = dbc.Receive(takedinShippingUnit, location);
                        var xml = GetMonitorData(receivedShippingUnit);
                        dbc.AddMonitorData(WorkstationType.Receiving, InstanceName, xml);
                        response.ResponseContent = new TrackingContract.ReceivingModule.ReceiveResponse()
                        {
                            StoreLocation = location,
                        };
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
                        dbc.AddMonitorData(WorkstationType.Receiving, InstanceName, xml);
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
        private string EventHubChannelName { get => $"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.ReceivingModule.MODULE_PREFIX}:{InstanceName}"; } 

        /// <summary>
        /// Visszadja a Monitorrekordba irandó adatokat
        /// </summary>
        /// <param name="receivedShippingUnit"></param>
        /// <returns></returns>
        private XElement GetMonitorData(ShippingUnit receivedShippingUnit)
        {
            var monitorDataList = new Dictionary<string, string>()
            {
                { "ShippingUnitId", receivedShippingUnit.ShippingUnitId },
                { "PartNumber", receivedShippingUnit.Part.PartNumber },
                { "PartDescription", receivedShippingUnit.Part.Description },
                { "Qty", receivedShippingUnit.Quantity.ToString() },
                { "StoreLocation", receivedShippingUnit.StoreLocation },
            };
            return MonitorDataContentHelper.CreateContent(monitorDataList);
        }

        /// <summary>
        /// Visszaadja a tárhelyet, ahová a beszállítói egység kerül (MOCK IMPLEMENTION ONLY!)
        /// </summary>
        /// <param name="takedinShippingUnit">a betárolandó beszállítói egység</param>
        /// <returns>térhely</returns>
        private string GetStoreLocation(ShippingUnit takedinShippingUnit) => Guid.NewGuid().ToString().Substring(0, 8);

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
                        TrackingContract.ReceivingModule.ReceiveRequest,
                        TrackingContract.ReceivingModule.ReceiveResponse>(EventHubChannelName, Receive);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ReceivingService()
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

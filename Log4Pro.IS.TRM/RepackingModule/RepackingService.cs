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

namespace Log4Pro.IS.TRM.RepackingModule
{
    /// <summary>
    /// Átcsomagolás modul implementáció
    /// </summary>
    internal class RepackingService : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RepackingService()
        {
            EventHubCore.RegisterHandler<RedisPubSubChannel, 
                TrackingContract.RepackingModule.AvailableQtyRequest, 
                TrackingContract.RepackingModule.AvailableQtyResponse>(EventHubChannelName, AvailableQty);
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                TrackingContract.RepackingModule.RepackRequest,
                TrackingContract.Response>(EventHubChannelName, Repacking);
        }

        /// <summary>
        /// Lekérdezi az átrakásra elérhető mennyiséget
        /// </summary>
        /// <param name="request">kérés</param>
        /// <returns>válasz</returns>
        private Response<TrackingContract.RepackingModule.AvailableQtyResponse> AvailableQty(
                    Request<TrackingContract.RepackingModule.AvailableQtyRequest, TrackingContract.RepackingModule.AvailableQtyResponse> request)
        {
            var response = request.MyResponse;
            try
            {
                using (var dbc = new ISTRMContext())
                {
                    var shippingUnit = dbc.ShippingUnits.FirstOrDefault(x => x.Active
                                                        && x.ShippingUnitId == request.RequestContent.ShippingUnitId
                                                        && x.ShippingUnitStatus == ShippingUnitStatus.Received.ToString());
                    if (shippingUnit == null)
                    {
                        throw new Exception($"This shipping unit is not exists, is not received, or is empty: {request.RequestContent.ShippingUnitId}");
                    }
                    else
                    {
                        response.ResponseContent = new TrackingContract.RepackingModule.AvailableQtyResponse()
                        {
                            AvailableQty = shippingUnit.Quantity,                            
                        };
                    }
                }                
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        /// <summary>
        /// Átcsomagolás művelet
        /// </summary>
        /// <param name="request">kérés</param>
        /// <returns>válasz</returns>
        private Response<TrackingContract.Response> Repacking(
                    Request<TrackingContract.RepackingModule.RepackRequest, TrackingContract.Response> request)
        {
            var response = request.MyResponse;
            try
            {
                if (string.IsNullOrEmpty(request.RequestContent.PackagingUnitId))
                {
                    throw new Exception($"Invalid package unit id!");
                }
                if (request.RequestContent.Qty <= 0)
                {
                    throw new Exception($"Invalid quantity: {request.RequestContent.Qty}");
                }
                using (var dbc = new ISTRMContext())
                {
                    var shippingUnit = dbc.ShippingUnits.FirstOrDefault(x => x.Active
                                                        && x.ShippingUnitId == request.RequestContent.ShippingUnitId
                                                        && x.ShippingUnitStatus == ShippingUnitStatus.Received.ToString());
                    if (shippingUnit == null)
                    {
                        throw new Exception($"This shipping unit is not exists, is not received, or is empty: {request.RequestContent.ShippingUnitId}");
                    }
                    else
                    {
                        var packagingUnit = dbc.RePack(shippingUnit, request.RequestContent.PackagingUnitId, request.RequestContent.Qty);
                        var xml = GetMonitorData(shippingUnit, packagingUnit, request.RequestContent.Qty);
                        dbc.AddMonitorData(WorkstationType.Repacking, InstanceName, xml);
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
                        dbc.AddMonitorData(WorkstationType.Repacking, InstanceName, xml);
                    }
                }
                catch { }
            }
            return response;
        }

        private XElement GetMonitorData(ShippingUnit shippingUnit, PackagingUnit packagingUnit, int qty)
        {
            var monitorDataList = new Dictionary<string, string>()
            {
                { "PartNumber", shippingUnit.Part.PartNumber },
                { "PartDescription", shippingUnit.Part.Description },
                { "ShippingUnitId", shippingUnit.ShippingUnitId },
                { "PackagingUnitId", packagingUnit.PackageUnitId },
                { "OriginalShippingUnitQty", shippingUnit.Quantity.ToString() },
                { "NewShippingUnitQty", (shippingUnit.Quantity - qty).ToString() },
                { "PackagingUnitQty", packagingUnit.Quantity.ToString() },
            };
            return MonitorDataContentHelper.CreateContent(monitorDataList);
        }

        /// <summary>
        /// Munkahely instance neve TODO: itt iplmentáld, ha később az konfigurálható kell legyen, vagy valami logika lapján kell képződjön!!!
        /// </summary>
        private string InstanceName { get => "demo"; }

        /// <summary>
        /// Eventhub csatorna neve
        /// </summary>
        private string EventHubChannelName { get => $"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.RepackingModule.MODULE_PREFIX}:{InstanceName}"; }

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
                        TrackingContract.RepackingModule.AvailableQtyRequest,
                        TrackingContract.RepackingModule.AvailableQtyResponse>(EventHubChannelName, AvailableQty);
                    EventHubCore.DropHandler<RedisPubSubChannel,
                        TrackingContract.RepackingModule.RepackRequest,
                        TrackingContract.Response>(EventHubChannelName, Repacking);
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RepackingService()
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

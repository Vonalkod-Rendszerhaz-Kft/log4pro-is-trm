using System;
using System.Collections.Generic;
using System.Linq;
using Log4Pro.IS.TRM.EventHubContract;
using Vrh.EventHub.Core;
using Vrh.EventHub.Protocols.RedisPubSub;
using Log4Pro.IS.TRM.DAL;

namespace Log4Pro.IS.TRM.TakeInModule
{
    /// <summary>
    /// Bevételezés alrendszer
    /// </summary>
    internal class TakeInService : IDisposable
    {
        public TakeInService()
        {
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                                            TrackingContract.TakeInModule.TakeInQueryRequest,
                                            TrackingContract.TakeInModule.TakeInQueryResponse>(EventHubChannelName, TakeInQuery);
            EventHubCore.RegisterHandler<RedisPubSubChannel,
                                            TrackingContract.TakeInModule.TakeInRequest,
                                            TrackingContract.Response>(EventHubChannelName, TakeIn);
        }

        /// <summary>
        /// Bevételezés azonosítás lekérdezés
        /// </summary>
        /// <param name="request">Kérés</param>
        /// <returns>Válasz</returns>
        private Response<TrackingContract.TakeInModule.TakeInQueryResponse> TakeInQuery(
            Request<TrackingContract.TakeInModule.TakeInQueryRequest, TrackingContract.TakeInModule.TakeInQueryResponse> request)
        {
            var response = request.MyResponse;
            try
            {
                var part = new SupplierShippingUnit(request.RequestContent.SupplierShippingUnitId);
                var responseContent = new TrackingContract.TakeInModule.TakeInQueryResponse();
                responseContent.Datas = new Dictionary<string, string>()
                {
                    { "mtsid", part.MTSId },
                    { "address", part.Slot },
                    { "c9023", part.FVS },
                    { "description", part.PartDescription },
                    { "partnumber", part.PartNumber },
                    { "quantity", part.Qty.ToString() },
                    { "msl", "" },
                    { "msg", "" },
                };
                response.ResponseContent = responseContent;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        /// <summary>
        /// Bevételezés
        /// </summary>
        /// <param name="request">kérés</param>
        /// <returns>válasz</returns>
        private Response<TrackingContract.Response> TakeIn(
            Request<TrackingContract.TakeInModule.TakeInRequest, TrackingContract.Response> request)
        {
            var response = request.MyResponse;
            try
            {
                var responseContent = new TrackingContract.Response();
                // TODO: Implementetion
                using(var dbc = new ISTRMContext())
                {
                    if (dbc.ShippingUnits.FirstOrDefault(x => x.ShippingUnitId == request.RequestContent.InternalShippingUnitId) != null)
                    {
                        throw new Exception($"This shipping unit id is already exists: {request.RequestContent.InternalShippingUnitId}");
                    }
                    else
                    {
                        var part = dbc.Parts.FirstOrDefault(x => x.PartNumber == request.RequestContent.PartNumber);
                        if (part == null)
                        {
                            throw new Exception($"This partnumber is not exists: {request.RequestContent.PartNumber}");
                        }
                        else
                        {
                            dbc.ShippingUnits.Add(
                                new ShippingUnit()
                                {
                                    Active = true,
                                    CreaterTransaction = new Transaction()
                                    {
                                        Type = TransactionType.SUTakeIn,
                                        Timestamp = DateTime.Now,
                                    },
                                    ExternalShippingUnitId = request.RequestContent.ExternalShippingUnitId,
                                    Part = part,
                                    Quantity = request.RequestContent.Qty,
                                    ShippingUnitId = request.RequestContent.InternalShippingUnitId,
                                    Status = ShippingUnitStatus.TakedIn,                                                               
                                });
                            dbc.SaveChanges();
                        }
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
        /// Munkahely instance neve TODO: itt iplmentáld, ha később az konfigurálható kell legyen, vagy valami logika lapján kell képződjön!!!
        /// </summary>
        private string InstanceName { get => "demo"; }

        /// <summary>
        /// Eventhub csatorna neve
        /// </summary>
        private string EventHubChannelName { get => $"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.TakeInModule.MODULE_PREFIX}:{InstanceName}"; }

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
                                                TrackingContract.TakeInModule.TakeInQueryRequest,
                                                TrackingContract.TakeInModule.TakeInQueryResponse>(EventHubChannelName, TakeInQuery);
                    EventHubCore.DropHandler<RedisPubSubChannel,
                                                TrackingContract.TakeInModule.TakeInRequest,
                                                TrackingContract.Response>(EventHubChannelName, TakeIn);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TakeInModule()
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

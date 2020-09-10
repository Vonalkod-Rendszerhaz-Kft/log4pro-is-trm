using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vrh.EventHub.Core;
using Vrh.EventHub.Protocols.RedisPubSub;
using Log4Pro.IS.TRM.EventHubContract;

namespace SzigetRendszer.Areas.ISTRM.Controllers
{
    public class APIController : Controller
    {
        // GET: ISTRM/API
        public ActionResult Index()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // POST: ISTRM/API/TakeInQuery
        [HttpPost]
        public ActionResult TakeInQuery(string supplierShippingUnitId)
        {
            TrackingContract.TakeInModule.TakeInQueryResponse response = null;
            var request = new TrackingContract.TakeInModule.TakeInQueryRequest()
            {
                SupplierShippingUnitId = supplierShippingUnitId,
            };
            try
            {
                response = EventHubCore.Call<RedisPubSubChannel,
                    TrackingContract.TakeInModule.TakeInQueryRequest,
                    TrackingContract.TakeInModule.TakeInQueryResponse>($"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.TakeInModule.MODULE_PREFIX}:demo", request);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // POST: ISTRM/API/TakeIn
        [HttpPost]
        public ActionResult TakeIn(string externalShippingUnitId, string internalShippingUnitId, string partNumber, int qty)
        {
            TrackingContract.Response response = null;
            var request = new TrackingContract.TakeInModule.TakeInRequest()
            {
                ExternalShippingUnitId = externalShippingUnitId,
                InternalShippingUnitId = internalShippingUnitId,
                PartNumber = partNumber,
                Qty = qty
            };
            try
            {
                response = EventHubCore.Call<RedisPubSubChannel,
                    TrackingContract.TakeInModule.TakeInRequest,
                    TrackingContract.Response>($"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.TakeInModule.MODULE_PREFIX}:demo", request);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // POST: ISTRM/API/Receive
        [HttpPost]
        public ActionResult Receive(string shippingUnitId)
        {
            TrackingContract.ReceivingModule.ReceiveResponse response = null;
            var request = new TrackingContract.ReceivingModule.ReceiveRequest()
            {
                ShippingUnitId = shippingUnitId
            };
            try
            {
                response = EventHubCore.Call<RedisPubSubChannel,
                    TrackingContract.ReceivingModule.ReceiveRequest,
                    TrackingContract.ReceivingModule.ReceiveResponse>($"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.ReceivingModule.MODULE_PREFIX}:demo", request);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // POST: ISTRM/API/AvailableQty
        [HttpPost]
        public ActionResult AvailableQty(string shippingUnitId, string packagingUnitId, int qty)
        {
            TrackingContract.RepackingModule.AvailableQtyResponse response = null;
            var request = new TrackingContract.RepackingModule.AvailableQtyRequest()
            {
                ShippingUnitId = shippingUnitId
            };
            try
            {
                response = EventHubCore.Call<RedisPubSubChannel,
                    TrackingContract.RepackingModule.AvailableQtyRequest,
                    TrackingContract.RepackingModule.AvailableQtyResponse>($"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.RepackingModule.MODULE_PREFIX}:demo", request);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // POST: ISTRM/API/Repack
        [HttpPost]
        public ActionResult Repack(string shippingUnitId, string packagingUnitId, int qty)
        {
            TrackingContract.Response response = null;
            var request = new TrackingContract.RepackingModule.RepackRequest()
            {
                ShippingUnitId = shippingUnitId,
                PackagingUnitId = packagingUnitId,
                Qty = qty
            };
            try
            {
                response = EventHubCore.Call<RedisPubSubChannel,
                    TrackingContract.RepackingModule.RepackRequest,
                    TrackingContract.Response>($"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.RepackingModule.MODULE_PREFIX}:demo", request);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // POST: ISTRM/API/PutOut
        [HttpPost]
        public ActionResult PutOut(string packagingUnitId)
        {
            TrackingContract.Response response = null;
            var request = new TrackingContract.PutOutModule.PutOutRequest()
            {
                PackagingUnitId = packagingUnitId
            };
            try
            {
                response = EventHubCore.Call<RedisPubSubChannel,
                    TrackingContract.PutOutModule.PutOutRequest,
                    TrackingContract.Response>($"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.PutOutModule.MODULE_PREFIX}:demo", request, new TimeSpan(0, 0, 5));
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

		// POST: ISTRM/API/KanbanStoreIn
		[HttpPost]
		public ActionResult KanbanStoreIn(string packagingUnitId)
		{
			try
			{
				var request = new TrackingContract.KanbanModule.SuccessStoreIn()
				{
					PackageUnitId = packagingUnitId,
				};
				EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.SuccessStoreIn>(
					$"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.KanbanModule.MODULE_PREFIX}:demo", 
					request);
				return Json(null, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(ex.Message, JsonRequestBehavior.AllowGet);
			}
		}

		// POST: ISTRM/API/KanbanStoreOut
		[HttpPost]
		public ActionResult KanbanStoreOut(string packagingUnitId)
		{
			try
			{
				var request = new TrackingContract.KanbanModule.SuccessStoreOut()
				{
					PackageUnitId = packagingUnitId,
				};
				EventHubCore.Send<RedisPubSubChannel, TrackingContract.KanbanModule.SuccessStoreOut>(
					$"{TrackingContract.CHANNEL_PREFIX}:{TrackingContract.KanbanModule.MODULE_PREFIX}:demo",
					request);
				return Json(null, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(ex.Message, JsonRequestBehavior.AllowGet);
			}
		}
	}
}
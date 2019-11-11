using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrh.EventHub.Intervention;

namespace Log4Pro.IS.TRM
{
    public class RequestDistributor : IDisposable
    {
        public RequestDistributor()
        {
            EventHubIntervention.RegisterInterventionChannel("IS-TRM", RequestDistributorMethod);
        }

        private Dictionary<string, string> RequestDistributorMethod(string intervention, Dictionary<string, string> parameters)
        {
            if (disposedValue)
            {
                throw new Exception("Tracking service is under stopping...");
            }
            // TODO: implement ditribution switc
            switch (intervention)
            {
                default:
                    throw new Exception($"Unknow request id: {intervention}");
            }
            return null;
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
                    EventHubIntervention.DropInterventionChannel("IS-TRM");
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RequestDistributor()
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

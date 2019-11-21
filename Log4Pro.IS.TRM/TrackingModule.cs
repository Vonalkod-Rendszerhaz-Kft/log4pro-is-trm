using System;
using Log4Pro.IS.TRM.TakeInModule;
using Log4Pro.IS.TRM.ReceivingModule;
using Log4Pro.IS.TRM.RepackingModule;
using Log4Pro.IS.TRM.PutOutModule;
using Log4Pro.IS.TRM.KanbanModule;

namespace Log4Pro.IS.TRM
{
    public class TrackingModule : IDisposable
    {
        /// <summary>
        /// Bevételezés almodul
        /// </summary>
        private readonly TakeInService takeInModule = new TakeInService();

        /// <summary>
        /// betárolás almodul
        /// </summary>
        private readonly ReceivingService receivingModule = new ReceivingService();

        /// <summary>
        /// Átcsomagolás almodul
        /// </summary>
        private readonly RepackingService repackingModule = new RepackingService();

        /// <summary>
        /// Kitárolás almodul
        /// </summary>
        private readonly PutOutService putOutModule = new PutOutService();

        /// <summary>
        /// Kanban almodul
        /// </summary>
        private readonly KanbanService kanbanModule = new KanbanService();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    takeInModule.Dispose();
                    receivingModule.Dispose();
                    repackingModule.Dispose();
                    putOutModule.Dispose();
                    kanbanModule.Dispose();
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

using System.Threading;
using fxcore2;

namespace Automat
{
    /// <summary>
    /// The class wrapping the response listener.
    /// </summary>
    internal class ResponseListener : IO2GResponseListener
    {
        private readonly SimpleLog _mSimpleLog;
        private readonly ResponseQueue _mResponseQueue;
        public EventWaitHandle ResponseHandle { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseListener"/> class.
        /// </summary>
        /// <param name="simpleLog">The log.</param>
        /// <param name="responseQueue">The response queue.</param>
        public ResponseListener(SimpleLog simpleLog, ResponseQueue responseQueue)
        {
            _mSimpleLog = simpleLog;
            _mResponseQueue = responseQueue;
            ResponseHandle = new AutoResetEvent(false);
        }
        /// <summary>
        /// When the request completed.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="response">The response.</param>
        public void onRequestCompleted(string requestId, O2GResponse response)
        {
            _mResponseQueue.Enqueue(response);
            ResponseHandle.Set();
        }
        /// <summary>
        /// When the request fails.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="error">The error.</param>
        public void onRequestFailed(string requestId, string error)
        {
            _mSimpleLog.Log("Request failed requestID={0} error={1}", requestId, error + ".");
        }
        /// <summary>
        /// Called when table updates.
        /// </summary>
        /// <param name="data">The data.</param>
        public void onTablesUpdates(O2GResponse data)
        {
            _mSimpleLog.Log("Data received.");
            _mResponseQueue.Enqueue(data);
            ResponseHandle.Set();
        }
    }
}

using System.Collections.Generic;
using fxcore2;

namespace Automat
{
    /// <summary>
    /// The class wrapping the response queue.
    /// </summary>
    internal class ResponseQueue
    {
        private readonly Queue<O2GResponse> _mQueue;
        private readonly object _mSyncRoot;
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseQueue"/> class.
        /// </summary>
        public ResponseQueue()
        {
            _mQueue = new Queue<O2GResponse>();
            _mSyncRoot = new object();
        }
        /// <summary>
        /// Gets the count in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock(_mSyncRoot)
                    return _mQueue.Count;
            }
        }
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (_mSyncRoot)
                _mQueue.Clear();
        }
        /// <summary>
        /// Determines whether [contains] [the specified response].
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified response]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(O2GResponse response)
        {
            lock(_mSyncRoot)
                return _mQueue.Contains(response);
        }
        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        public O2GResponse Dequeue()
        {
            lock(_mSyncRoot)
                return _mQueue.Dequeue();
        }
        /// <summary>
        /// Enqueues the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        public void Enqueue(O2GResponse response)
        {
            lock(_mSyncRoot)
                _mQueue.Enqueue(response);
        }
    }
}

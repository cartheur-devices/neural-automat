using System.Threading;
using fxcore2;

namespace Automat
{
    internal class SessionStatusListener : IO2GSessionStatus
    {
        private readonly SimpleLog _mSimpleLog;
        private readonly O2GSession _mSession;
        private readonly LoginInfo _mLoginInfo;
        public EventWaitHandle StatusHandle { get; private set; }
        public EventWaitHandle StopHandle { get; private set; }
        public EventWaitHandle ConnectedHandle { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStatusListener"/> class.
        /// </summary>
        /// <param name="simpleLog">The log.</param>
        /// <param name="session">The session.</param>
        /// <param name="loginInfo">The login information object.</param>
        public SessionStatusListener(SimpleLog simpleLog, O2GSession session, LoginInfo loginInfo)
        {
            _mSimpleLog = simpleLog;
            _mSession = session;
            _mLoginInfo = loginInfo;
            StatusHandle = new AutoResetEvent(false);
            StopHandle = new AutoResetEvent(false);
            ConnectedHandle = new AutoResetEvent(false);
        }
        /// <summary>
        /// On login failed meethod.
        /// </summary>
        /// <param name="sError">The s error.</param>
        public void onLoginFailed(string sError)
        {
            _mSimpleLog.Log(sError);
            StopHandle.Set();
        }
        /// <summary>
        /// On session status changed
        /// </summary>
        /// <param name="eStatus">The e status.</param>
        public void onSessionStatusChanged(O2GSessionStatusCode eStatus)
        {
            switch (eStatus)
            {
                case O2GSessionStatusCode.Connecting:
                    _mSimpleLog.Log("Status: Connecting...");
                    break;

                case O2GSessionStatusCode.Connected:
                    // "How to login" : Use status listener onSessionStatusChanged function to capture Connected event.
                    _mSimpleLog.Log("Status: Connected.");
                    ConnectedHandle.Set();
                    break;

                case O2GSessionStatusCode.Disconnecting:
                    _mSimpleLog.Log("Status: Disconnecting...");
                    break;

                case O2GSessionStatusCode.Disconnected:
                    // "How to login" : Use status listener onSessionStatusChanged function to capture Disconnected event.
                    _mSimpleLog.Log("Status: Disconnected.");
                    StatusHandle = StopHandle;
                    break;

                case O2GSessionStatusCode.Reconnecting:
                    _mSimpleLog.Log("Status: Reconnecting...");
                    break;

                case O2GSessionStatusCode.SessionLost:
                    _mSimpleLog.Log("Status: Session Lost.");
                    StatusHandle = StopHandle;
                    break;

                // "How to login" : If you have more than one trading sessions or pin is required to login, 
                //                  you have to catch the event TradingSessionRequested in onSessionStatusChanged function of your status listener. 
                case O2GSessionStatusCode.TradingSessionRequested:
                    _mSimpleLog.Log("Status: TradingSessionRequested.");
                    var sTradingSessionId = _mLoginInfo.TradingSessionId;
                    if (string.IsNullOrEmpty(sTradingSessionId))
                    {
                        // "How to login" : In that case get IO2GSessionDescriptorCollection.
                        var descriptors = _mSession.getTradingSessionDescriptors();

                        // "How to login" : Then process elements of this collection 
                        if (descriptors.Count > 0)
                            sTradingSessionId = descriptors[0].Id;
                    }

                    // "How to login" : Finally set trading session using session Id and pin
                    _mSession.setTradingSession(sTradingSessionId, _mLoginInfo.TradingSessionPin);
                    break;
            }

            StatusHandle.Set();
        }
    }
}

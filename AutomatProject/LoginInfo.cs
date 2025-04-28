namespace Automat
{
    /// <summary>
    /// The class wrapping the login information for the connection to the forex server.
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// The username field.
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// The password field.
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// The connection field.
        /// </summary>
        public string Connection { get; private set; }
        /// <summary>
        /// The type of connection.
        /// </summary>
        /// <value>
        /// 'Demo' or 'Real'.
        /// </value>
        public string ConnectionType { get; private set; }
        /// <summary>
        /// The trading session ID.
        /// </summary>
        public string TradingSessionId { get; private set; }
        /// <summary>
        /// The trading session pin.
        /// </summary>
        public string TradingSessionPin { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginInfo"/> class.
        /// </summary>
        /// <param name="sUsername">The username.</param>
        /// <param name="sPassword">The password.</param>
        /// <param name="sConnection">The connection.</param>
        /// <param name="sConnectionType">Type of the connection - Demo or Real.</param>
        /// <param name="sTradingSessionId">The trading session ID.</param>
        /// <param name="sTradingSessionPin">The trading session pin.</param>
        public LoginInfo(
            string sUsername,
            string sPassword,
            string sConnection,
            string sConnectionType,
            string sTradingSessionId,
            string sTradingSessionPin)
        {
            Username = sUsername;
            Password = sPassword;
            Connection = sConnection;
            ConnectionType = sConnectionType;
            TradingSessionId = sTradingSessionId;
            TradingSessionPin = sTradingSessionPin;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginInfo"/> class.
        /// </summary>
        /// <param name="sUsername">The username.</param>
        /// <param name="sPassword">The password.</param>
        /// <param name="sConnection">The connection.</param>
        /// <param name="sConnectionType">The type of the connection - Demo or Real.</param>
        public LoginInfo(
            string sUsername,
            string sPassword,
            string sConnection,
            string sConnectionType)
            : this(sUsername, sPassword, sConnection, sConnectionType, string.Empty, string.Empty)
            { }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}",
                Username,
                new string('*', Password.Length),
                Connection,
                ConnectionType,
                TradingSessionId,
                TradingSessionPin);
        }
    }
}

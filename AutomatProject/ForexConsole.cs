using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using System.Windows.Forms;
using fxcore2;

namespace Automat
{
    public partial class ForexConsole : Form, IO2GSessionStatus
    {
        public ForexConsole()
        {
            InitializeComponent();
            SyncResponseEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            numberOfBars.Text = "0";
            dateTimeFrom.Value = DateTime.Now - TimeSpan.FromDays(1);
            getButton.Enabled = false; // not working as desired presently.
            dateTimeFrom.Format = DateTimePickerFormat.Custom;
            dateTimeFrom.CustomFormat = "dd-MMM-yyyy HH:mm";
            dateTimeTo.Format = DateTimePickerFormat.Custom;
            dateTimeTo.CustomFormat = "dd-MMM-yyyy HH:mm"; 
        }

        public O2GSession Session;
        public EventWaitHandle SyncResponseEvent = null;
        readonly object _sessionStatus = new object();

        public O2GSessionStatusCode Status
        {
            get
            {
                O2GSessionStatusCode status;
                lock (_sessionStatus)
                {
                    status = _mSessionStatus;
                }
                return status;
            }
            private set
            {
                lock (_sessionStatus)
                {
                    _mSessionStatus = value;
                }
            }
        }
        O2GSessionStatusCode _mSessionStatus;

        private readonly SimpleLog _log =
            //new SimpleLog(@"c:\aeonWorkingDirectory\websites\forex-anew\Automat Project\log\forexLog.txt");
            new SimpleLog(@"c:\aeonWorkingDirectory\bph\Automat Project\log\forexLog.txt");
        
        private const string UserName = "D171941699001";
        private const string Password = "3827";
        private const string Url = "http://www.fxcorporate.com/Hosts.jsp";
        private const string Connection = "Demo";

        private int Bars;

        public struct ForexDataElement
        {
            public DateTime Date;
            public double BidOpen;
            public double BidLow;
            public double BidHigh;
            public double BidClose;
            public double Volume;
        };

        public struct Candlestick
        {
            public double BidLow;
            public double BidHigh;
            //public double BidClose;
        };
        /// <summary>
        /// The method running when logging in.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LoginButtonClick(object sender, EventArgs e)
        {
            if (numberOfBars.Text != "0")
                try
                {
                    var responseQueue = new ResponseQueue();
                    var responseListener = new ResponseListener(_log, responseQueue);
                    Session = O2GTransport.createSession(); //create IO2GSession object
                    Session.subscribeResponse(responseListener);
                    //subscribe to session status events
                    Session.LoginFailed += SessionLoginFailed;
                    Session.SessionStatusChanged += SessionSessionStatusChanged;
                    Session.RequestCompleted += SessionRequestCompleted;
                    Session.RequestFailed += SessionRequestFailed;

                    //Please specify valid username and password
                    Session.login(UserName, Password, Url, Connection);

                    //Waiting for result of async login           
                    if (!SyncResponseEvent.WaitOne(5000) || Status != O2GSessionStatusCode.Connected)
                    {
                        return;
                    }
                    if (Status != O2GSessionStatusCode.Connected) return;
                    try
                    {
                        loginButton.Enabled = false;
                        O2GRequestFactory factory = Session.getRequestFactory();
                        O2GTimeframeCollection timeframes = factory.Timeframes;
                        var tfo = timeframes["m1"];
                        var currencyFrom = currencyOne.SelectedItem;
                        var currencyTo = currencyTwo.SelectedItem;
                        string currencyRelation = currencyFrom + "/" + currencyTo;
                        Bars = Convert.ToInt32(numberOfBars.Text);
                        O2GRequest request = factory.createMarketDataSnapshotRequestInstrument(currencyRelation, tfo, Bars);
                        CreateFactorySessionHistoricalForex(request, factory, dateTimeFrom.Value, dateTimeTo.Value);

                    }
                    catch (Exception)
                    {
                        Stop();
                        throw;
                    }
                }
                catch (Exception)
                {
                    Stop();
                    throw;
                }
            else
                labelStatus.Text = "Bars cannot be zero.";
        }
        /// <summary>
        /// The method which runs when the GetData button is clicked..
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void GetButtonClick(object sender, EventArgs e)
        {
            //ProcessArray.TestArray();

            O2GRequestFactory factory = Session.getRequestFactory();
            O2GTimeframeCollection timeframes = factory.Timeframes;
            var tfo = timeframes["m1"];
            var currencyFrom = currencyOne.SelectedItem;
            var currencyTo = currencyTwo.SelectedItem;
            var bars = Convert.ToInt32(numberOfBars.Text);
            //O2GRequest request = factory.createMarketDataSnapshotRequestInstrument(currencyFrom + "@/" + currencyTo, tfo, Bars);
            O2GRequest request = factory.createMarketDataSnapshotRequestInstrument("EUR/USD", tfo, Bars);
            CreateFactorySessionHistoricalForex(request, factory, dateTimeFrom.Value, dateTimeTo.Value);
            // Send to an array stored in memory for the network to process.
            //var solution = Solution.SearchSolution();
            //var hold = "";
        }
        /// <summary>
        /// Logout and Stop this instance.
        /// </summary>
        public void Stop()
        {
            if (Session != null)
            {
                Session.logout();
                SyncResponseEvent.WaitOne(5000); //wait for logout completion during 5 seconds

                Session.LoginFailed -= SessionLoginFailed;
                Session.SessionStatusChanged -= SessionSessionStatusChanged;

                Session.Dispose();
                Session = null;//to avoid second time stop
                _mSessionStatus = O2GSessionStatusCode.Disconnected; //for getStatus()
            }
        }
        /// <summary>
        /// Creates the factory session for historical forex.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="from">time from.</param>
        /// <param name="to">time to.</param>
        public void CreateFactorySessionHistoricalForex(O2GRequest request, O2GRequestFactory factory, DateTime from, DateTime to)
        {
            factory.fillMarketDataSnapshotRequestTime(request, from, to, true);
            Session.sendRequest(request);
        }
        /// <summary>
        /// Logs out from the session.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LogoutButtonClick(object sender, EventArgs e)
        {
            Session.logout();
            loginButton.Enabled = true;
            Echo("Session terminated.");
            Session.Dispose();
        }
        /// <summary>
        /// Completes the session request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="fxcore2.RequestCompletedEventArgs"/> instance containing the event data.</param>
        private void SessionRequestCompleted(object sender, RequestCompletedEventArgs e)
        {
            var readerFactory = Session.getResponseReaderFactory();

            if (e.Response.Type == O2GResponseType.MarketDataSnapshot)
            {
                var responseReader = readerFactory.createMarketDataSnapshotReader(e.Response);
                var datePositionTable = new Hashtable();
                var dataBlock = new List<double[]>();

                for (var i = 0; i < responseReader.Count; i++)
                {
                    var date = responseReader.getDate(i);
                    var bidOpen = responseReader.getBidOpen(i);
                    var bidLow = responseReader.getBidLow(i);
                    var bidHigh = responseReader.getBidHigh(i);
                    var bidClose = responseReader.getBidClose(i);
                    var volume = responseReader.getVolume(i);

                    StoreForexData(date, bidOpen, bidLow, bidHigh, bidClose, volume);

                    //var dataPiece = new[] { bidLow, bidHigh, bidOpen, bidClose };
                    
                    //for (var j = 0; j < Bars; j++)
                    //{
                    //    dataBlock.Add(dataPiece);
                    //    datePositionTable[j] = date;
                    //    // Now send to the network for processing--what format can the network learn?.
                    //}
                    //var solution = Solution.SearchSolution(dataBlock, datePositionTable, Bars);
                    //var hold = "";
                }

                //StoreForexData(date, bidOpen, bidLow, bidHigh, bidClose, volume);
                    //Echo("Data received.");
                
            }
            //Echo("Data session finished."); Thread exception.

            SyncResponseEvent.Set(); //signal for waiter of response
        }
        /// <summary>
        /// Stores the forex data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="bidOpen">The bid open.</param>
        /// <param name="bidLow">The bid low.</param>
        /// <param name="bidHigh">The bid high.</param>
        /// <param name="bidClose">The bid close.</param>
        /// <param name="volume">The volume.</param>
        protected void StoreForexData(DateTime date, double bidOpen, double bidLow, double bidHigh, double bidClose, int volume)
        {
            //const string connectionstring = @"Data Source=c:\aeonWorkingDirectory\websites\forex-anew\Automat Project\db\ForexData.db;Version=3";
            const string connectionstring = @"Data Source=c:\aeonWorkingDirectory\bph\Automat Project\db\ForexData1000.db;Version=3";

            try
            {
                var conn = new SQLiteConnection(connectionstring);
                conn.Open();
                var cmd = conn.CreateCommand();
                var trans = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandText = "INSERT INTO ForexDataStore (date, bidOpen, " +
                    "bidLow, bidHigh, bidClose, volume) VALUES (" + "'" + date + "'" + ", '" + bidOpen + "', '" +
                    bidLow + "', '" + bidHigh + "', '" + bidClose + "', '" + volume + "')";
                cmd.ExecuteNonQuery();
                trans.Commit();
                //trans.Rollback();
                trans.Dispose();
                conn.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("no longer useable") == false)
                {
                    MessageBox.Show("Exception: " + e.Message);
                }
            }
        }

        #region Full double[][] datasets

        private void SessionRequestCompletedFull(object sender, RequestCompletedEventArgs e)
        {
            var readerFactory = Session.getResponseReaderFactory();

            if (e.Response.Type == O2GResponseType.MarketDataSnapshot)
            {
                var responseReader = readerFactory.createMarketDataSnapshotReader(e.Response);

                for (var i = 0; i < responseReader.Count; i++)
                {
                    var date = responseReader.getDate(i);
                    var bidOpen = responseReader.getBidOpen(i);
                    var bidLow = responseReader.getBidLow(i);
                    var bidHigh = responseReader.getBidHigh(i);
                    var bidClose = responseReader.getBidClose(i);
                    var volume = responseReader.getVolume(i);

                    var dataPoints = new double[Bars];

                    for (var k = 0; k < Bars; k++)
                    {
                        //bidClose = dataPoints[k];
                    }
                    var hold = "";
                    // Send to the struct.
                    //var dataBlock = new List<ForexDataElement>();
                    var dataCandlestick = new List<Candlestick>();
                    var datePositionTable = new Hashtable();
                    var dataBlock = new List<double[]>();
                    var dataPiece = new[] { bidLow, bidHigh };
                    for (var j = 0; j < Bars; j++)
                    {
                        // dataBlock.Add(new ForexDataElement { Date = date, BidOpen = bidOpen, BidLow = bidLow, BidHigh = bidHigh, BidClose = bidClose, Volume = volume });
                        dataCandlestick.Add(new Candlestick { BidLow = bidLow, BidHigh = bidHigh });
                        dataBlock.Add(dataPiece);
                        datePositionTable[j] = date;
                        // Now send to the network for processing--what format can the network learn?.
                    }
                    //var solution = Solution.SearchSolution(dataBlock, datePositionTable);
                    //var hold = "";
                }

                //StoreForexData(date, bidOpen, bidLow, bidHigh, bidClose, volume);
                //Echo("Data received.");

            }
            //Echo("Data session finished."); Thread exception.

            SyncResponseEvent.Set(); //signal for waiter of response
        }

        #endregion

        void SessionRequestFailed(object sender, RequestFailedEventArgs e)
        {
            Stop();
            throw new Exception(e.Error.ToString());
        }
        void SessionSessionStatusChanged(object sender, SessionStatusEventArgs e)
        {
            lock (_sessionStatus)
            {
                _mSessionStatus = e.SessionStatus;
            }

            switch (e.SessionStatus)
            {
                case O2GSessionStatusCode.Connected:
                case O2GSessionStatusCode.Disconnected:
                    SyncResponseEvent.Set();
                    break;
            }
        }

        void SessionLoginFailed(object sender, LoginFailedEventArgs e)
        {
            lock (_sessionStatus)
            {
                _mSessionStatus = O2GSessionStatusCode.Disconnected;
            }

            SyncResponseEvent.Set();
        }
        public void onLoginFailed(string error)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Echoes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Echo(string message)
        {
            // ReSharper disable LocalizableElement - Does not work in console unless in this syntax.
            reportingBox.Text += message + "\r\n";
            // ReSharper restore LocalizableElement
            reportingBox.SelectionStart = reportingBox.Text.Length;
            reportingBox.ScrollToCaret();
        }
        /// <summary>
        /// The session status changed event.
        /// </summary>
        /// <param name="status">The status.</param>
        public void onSessionStatusChanged(O2GSessionStatusCode status)
        {
            var field = status;
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Session != null)
            { Session.Dispose(); }
            Close();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            if (Session != null)
            { Session.Dispose(); }
            Close();
        }
    }
}

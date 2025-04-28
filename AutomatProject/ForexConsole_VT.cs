using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using VTAPI;

namespace Automat
{
    public partial class ForexConsole : Form
    {
        public ForexConsole()
        {
            InitializeComponent();
        }

        public VT_APIClass Api = new VT_APIClass();
        const int ServerId = 0;
        const string UserName = "ezraeurope@yahoo.com";
        const string Password = "xw7b6sn5";
        Timer _serverTimer = new Timer();
        Timer _forexTimer = new Timer();
        Enum _marketStatus = MarketStatus.MS_UNKNOWN;
        

        private void LoginButtonClick(object sender, EventArgs e)
        {
            Api = new VT_APIClass();
            AssignEvents();
            VtapiLogin();
            Echo("VTAPI Initialized.");
        }

        private void LogoutButtonClick(object sender, EventArgs e)
        {
            _serverTimer.Enabled = false;
            _forexTimer.Enabled = false;
            _serverTimer.Dispose();
            _forexTimer.Dispose();
            Api.Logout();
            labelStatus.Text = "Disconnected from server.";
        }

        protected void VtapiLogin()
        {
            try
            {
                Echo("Connecting " + UserName + " ...");
                Api.Login(UserName, Password, ServerId);
                Echo("User logged in.");
                labelStatus.Text = "Connected, server time " + Api.GetServerTime() + " (GMT -5)";
                _marketStatus = this.Api.MarketStatus;
                //API.MarketStatus returns MS_OPEN although is closed. Might be due to demo account access.
                //The market is open 24 hours a day from 1700 EST (GMT-5) on Sunday until 1600 EST (GMT-5) Friday. 
                marketStatus.Text = "Market: " + _marketStatus;
                _serverTimer.Enabled = true;
                _serverTimer.Interval = 1000;
                _serverTimer.Tick += ServerTimerTick;

            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }

        }

        protected void ServerTimerTick(object sender, EventArgs e)
        {
            labelStatus.Text = "";
            labelStatus.Text = "Connected, server time " + Api.GetServerTime() + " (GMT -5)";
        }

        private static void ShowError(string errormessage)
        {
            MessageBox.Show(errormessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void Echo(string message)
        {
            reportingBox.Text += message + "\r\n";
            reportingBox.SelectionStart = reportingBox.Text.Length;
            reportingBox.ScrollToCaret();
        }

        private void AssignEvents()
        {
            Api.OnNewServerMessage += OnNewServerMessage;
        }

        private void OnNewServerMessage(int msgIndex)
        {
            var msgText = Api.ServerMessages.Items[msgIndex].Details[0];
            Echo(String.Format("--- Event: '{0}' ---", msgText));
            //Echo(String.Format("--- Event: '{0}' ---", msg_index));
        }

        private void GetButtonClick(object sender, EventArgs e)
        {
            _forexTimer.Enabled = true;
            _forexTimer.Interval = 60000;
            _forexTimer.Tick += ForexTimerTick;
            object resulting = Api.GetInstrumentByCurrency("EUR", "USD").GetHistory(1, false, 1);
            if (resulting != null)
            {
                var document = new XmlDocument();
                document.LoadXml(resulting.ToString());
                document.Clone();
                ParseXmlFields(document);
            }
        }

        protected void ForexTimerTick(object sender, EventArgs e)
        {
            object obj = null;
            obj = Api.GetInstrumentByCurrency("EUR", "USD").GetHistory(1, false, 1);
            var document = new XmlDocument();
            if (obj == null) return;
            document.LoadXml(obj.ToString());
            ParseXmlFields(document);
        }

        protected void ParseXmlFields(XmlDocument input)
        {
                var nodelist = input.SelectNodes("Candles/Candle");

                for (var i = 0; i < nodelist.Count; i++)
                {
                    var node = nodelist[i];
                    var attCol = node.Attributes;

                    //int candleid = i + 1;
                    if (attCol != null)
                    {
                        var starttime = attCol[0].Value;
                        var endtime = attCol[1].Value;
                        var openprice = attCol[2].Value;
                        var min = attCol[3].Value;
                        var max = attCol[4].Value;
                        var last = attCol[5].Value;
                        var volume = attCol[6].Value;

                        var stampedStartTime = DateTime.Parse(starttime);
                        var stampedEndTime = DateTime.Parse(endtime);
                        var convertedOpen = Convert.ToDouble(openprice);
                        var convertedMin = Convert.ToDouble(min);
                        var convertedMax = Convert.ToDouble(max);
                        var convertedLast = Convert.ToDouble(last);
                        var convertedVol = Convert.ToInt32(volume);

                        PositTypesInDb(stampedStartTime, stampedEndTime, convertedOpen, convertedMin, convertedMax, convertedLast, convertedVol);
                        forexResultLabel.Text = "";
                        forexResultLabel.Text = "Datum at market time " + endtime + " (GMT-5)";
                    }
                    forexActivity.Text += "Datum stored." + "\r\n";
                }

                //MessageBox.Show("Congratulations! Xml stream-data is in the database.");
        }

        protected void PositTypesInDb(DateTime starttime, DateTime endtime, double open, double min, double max, double last, int volume)
        {
            const string connectionstring = @"Data Source=ANDORA;Initial Catalog=ForexData;Integrated Security=SSPI";

            try
            {
                var conn = new SqlConnection(connectionstring);
                conn.Open();
                var cmd = conn.CreateCommand();
                var trans = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandText = "INSERT INTO ForexCandlesMarch2009_1 (StartTime, EndTime, OpenPrice, " +
                    "Min, Max, Last, Volume) VALUES (" + "'" + starttime + "'" + ", '" + endtime + "', '" +
                    open + "', '" + min + "', '" + max + "', '" + last + "', '" + volume + "')";
                cmd.ExecuteNonQuery();
                trans.Commit();
                trans.Rollback();
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

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (Api != null)
            {
                Api.Finalize();
            }
            Close();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            if (Api != null)
            {
                Api.Finalize();
            }
            if (_forexTimer != null && (_serverTimer != null && ((_serverTimer.Enabled = true) || (_forexTimer.Enabled = true))))
            {
                _forexTimer.Dispose();
                _serverTimer.Dispose();
            }
            Close();
        }

    }
}

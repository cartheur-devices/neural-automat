using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NeuralAlgorithm.Numeric;
using NeuralAlgorithm.Controls;
using NeuralAlgorithm.ActivationFunctions;
using NeuralAlgorithm.Core.Learning;
using NeuralAlgorithm.Core.Networks;

namespace DataSorting
{
    public partial class ManagementForm : Form
    {

        double[,] tempData = null;
        private double _learningRate = 0.1;
        private double _momentum = 0.0;
        private double _sigmoidAlphaValue = 2.0;
        private int _neuronsInFirstLayer = 20;
        private int _iterations = 0;

        private Thread _workerThread = null;
        private bool _needToStop = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementForm"/> class.
        /// </summary>
        public ManagementForm()
        {
            InitializeComponent();
            iterationsTextBox.Text = "1";
            displayChart.AddDataSeries("data", Color.Red, Chart.SeriesType.Line, 1);
            displayChart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);
            ParseField();
        }
        /// <summary>
        /// Retrieves the data for the chart and the NN.
        /// </summary>
        /// <returns></returns>
        public DataSet RetrieveData()
        {
            var ds = new DataSet();
            const string connectionstring = @"Data Source=c:\aeonWorkingDirectory\bph\NeuralAutomat\DataSorting\db\ForexData6967.db;Version=3";

            try
            {
                // Recall the forex data.
                var conn = new SQLiteConnection(connectionstring);
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ForexDataStore";
                var adapt = new SQLiteDataAdapter(cmd);
                adapt.Fill(ds);
                conn.Close();
                adapt.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in recalling forex data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            return ds;
        }
        /// <summary>
        /// Parses the fields of the data.
        /// </summary>
        /// <returns></returns>
        protected List<double[]> ParseField()
        {
            var input = RetrieveData();
            var dataBlock = new List<double[]>();

            for (var i = 0; i < input.Tables[0].Rows.Count; i++)
            {
                var date = input.Tables[0].Rows[i]["date"];
                var bidLow = Convert.ToDouble(input.Tables[0].Rows[i]["bidLow"]);
                var bidHigh = Convert.ToDouble(input.Tables[0].Rows[i]["bidHigh"]);
                var bidClose = Convert.ToDouble(input.Tables[0].Rows[i]["bidClose"]);
                
                var dataPiece = new[] { i, bidLow, bidHigh, bidClose };
                dataBlock.Add(dataPiece);
                
            }
            forexWeekEnding.Text = input.Tables[0].Rows[0]["date"].ToString();
            forexWeekBeginning.Text = input.Tables[0].Rows[input.Tables[0].Rows.Count - 1]["date"].ToString();
            PickDataBidClose(dataBlock);
            return dataBlock;

        }
        // Pick the bid close data for the chart.
        protected double[,] PickDataBidClose(List<double[]> dataList)
        {
            tempData = new double[dataList.Count, 2];
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            //data = new double[dataList.Count, 2];
            for (int i = 0; i < dataList.Count; )
            {
                var dataPiece = dataList[i];
                var bidClose = dataPiece[3];
                tempData[i, 0] = i;
                tempData[i, 1] = bidClose;

                // search for min value
                if (tempData[i, 0] < minX)
                    minX = tempData[i, 0];
                // search for max value
                if (tempData[i, 0] > maxX)
                    maxX = tempData[i, 0];
                
                i++;
            }
            //Array.Copy(tempData, 0, data, 0, dataList.Count);
            CreateChart(tempData, minX, maxX);
            return tempData;
        }
        // Display data on a chart, given the progression of i.
        protected void CreateChart(double[,] data, double minX, double maxX)
        {
            //displayChart.RangeX = new NeuralAlgorithm.Numeric.DoubleRange(0, data.Length - 1);
            displayChart.RangeX = new DoubleRange(minX, maxX);
            displayChart.UpdateDataSeries("data", data);
        }
        // Search the solution based on the data.
        protected void SearchSolution()
        {
            _iterations = Convert.ToInt32(iterationsTextBox.Text);
            Cursor.Current = Cursors.WaitCursor;
            // number of learning samples
            int samples = tempData.GetLength(0);
            // data transformation factor
            double yFactor = 1.7 / displayChart.RangeY.Length;
            double yMin = displayChart.RangeY.Min;
            double xFactor = 2.0 / displayChart.RangeX.Length;
            double xMin = displayChart.RangeX.Min;

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[1];
                output[i] = new double[1];

                // set input
                input[i][0] = (tempData[i, 0] - xMin) * xFactor - 1.0;
                // set output
                output[i][0] = (tempData[i, 1] - yMin) * yFactor - 0.85;
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(new BipolarSigmoidFunction(_sigmoidAlphaValue), 1, _neuronsInFirstLayer, 1);
            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);
            // set learning rate and momentum
            teacher.LearningRate = _learningRate;
            teacher.Momentum = _momentum;

            // iterations
            int iteration = 1;

            // solution array
            double[,] solution = new double[samples, 2];
            double[] networkInput = new double[1];

            // calculate X values to be used with solution function
            for (int j = 0; j < samples; j++)
            {
                solution[j, 0] = displayChart.RangeX.Min + (double)j * displayChart.RangeX.Length / samples - 1;
            }

            // loop
            while (!_needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution
                for (int j = 0; j < samples; j++)
                {
                    networkInput[0] = (solution[j, 0] - xMin) * xFactor - 1.0;
                    solution[j, 1] = (network.Compute(networkInput)[0] + 0.85) / yFactor + yMin;
                }
                displayChart.UpdateDataSeries("solution", solution);
                // calculate error
                double learningError = 0.0;
                for (int j = 0, k = tempData.GetLength(0); j < k; j++)
                {
                    networkInput[0] = input[j][0];
                    learningError += Math.Abs(tempData[j, 1] - ((network.Compute(networkInput)[0] + 0.85) / yFactor + yMin));
                }

                // set current iteration's info
                //THREAD
                //currentIterationBox.Text = iteration.ToString();
                //currentErrorBox.Text = learningError.ToString("F3");

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((_iterations != 0) && (iteration > _iterations))
                {
                    Cursor.Current = Cursors.Default;
                    break;
                }
                    
            }

            // enable settings controls
            //EnableControls(true);
        }
        // Update settings controls
        private void UpdateSettings()
        {
            //learningRateBox.Text = _learningRate.ToString();
            //momentumBox.Text = _momentum.ToString();
            //alphaBox.Text = _sigmoidAlphaValue.ToString();
            //neuronsBox.Text = _neuronsInFirstLayer.ToString();
            //iterationsBox.Text = _iterations.ToString();
        }
        /// <summary>
        /// Handles the Click event of the closeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Handles the Click event of the searchSolutionButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void searchSolutionButton_Click(object sender, EventArgs e)
        {
            SearchSolution();
        }
    }
}

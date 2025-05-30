using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using NeuralAlgorithm;
using NeuralAlgorithm.ActivationFunctions;
using NeuralAlgorithm.Core.Learning;
using NeuralAlgorithm.Controls;
using NeuralAlgorithm.Core.Networks;
using NeuralAlgorithm.Numeric;

namespace Approximation
{
    /// <summary>
    /// A GUI for the Approximation neural network.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {

        #region WinForm items

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView dataList;
        private System.Windows.Forms.Button loadDataButton;
        private System.Windows.Forms.ColumnHeader xColumnHeader;
        private System.Windows.Forms.ColumnHeader yColumnHeader;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox2;
        private NeuralAlgorithm.Controls.Chart chart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox momentumBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox alphaBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox learningRateBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox iterationsBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox currentErrorBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox neuronsBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        private double[,] _data = null;

        private double _learningRate = 0.1;
        private double _momentum = 0.0;
        private double _sigmoidAlphaValue = 2.0;
        private int _neuronsInFirstLayer = 20;
        private int _iterations = 1000;

        private Thread _workerThread = null;
        private bool _needToStop = false;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // init chart control
            chart.AddDataSeries("data", Color.Red, Chart.SeriesType.Dots, 5);
            chart.AddDataSeries("solution", Color.Blue, Chart.SeriesType.Line, 1);

            // init controls
            UpdateSettings();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataList = new System.Windows.Forms.ListView();
            this.xColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.yColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.loadDataButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chart = new NeuralAlgorithm.Controls.Chart();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.neuronsBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.momentumBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.alphaBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.learningRateBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.iterationsBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.currentErrorBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataList);
            this.groupBox1.Controls.Add(this.loadDataButton);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 320);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // dataList
            // 
            this.dataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.xColumnHeader,
            this.yColumnHeader});
            this.dataList.FullRowSelect = true;
            this.dataList.GridLines = true;
            this.dataList.Location = new System.Drawing.Point(10, 20);
            this.dataList.Name = "dataList";
            this.dataList.Size = new System.Drawing.Size(160, 255);
            this.dataList.TabIndex = 0;
            this.dataList.UseCompatibleStateImageBehavior = false;
            this.dataList.View = System.Windows.Forms.View.Details;
            // 
            // xColumnHeader
            // 
            this.xColumnHeader.Text = "X";
            // 
            // yColumnHeader
            // 
            this.yColumnHeader.Text = "Y";
            // 
            // loadDataButton
            // 
            this.loadDataButton.Location = new System.Drawing.Point(10, 285);
            this.loadDataButton.Name = "loadDataButton";
            this.loadDataButton.Size = new System.Drawing.Size(75, 23);
            this.loadDataButton.TabIndex = 1;
            this.loadDataButton.Text = "&Load";
            this.loadDataButton.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
            this.openFileDialog.Title = "Select data file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chart);
            this.groupBox2.Location = new System.Drawing.Point(200, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 320);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Function";
            // 
            // chart
            // 
            this.chart.Location = new System.Drawing.Point(10, 20);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(280, 290);
            this.chart.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.neuronsBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.momentumBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.alphaBox);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.learningRateBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.iterationsBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(510, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(195, 195);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // neuronsBox
            // 
            this.neuronsBox.Location = new System.Drawing.Point(125, 95);
            this.neuronsBox.Name = "neuronsBox";
            this.neuronsBox.Size = new System.Drawing.Size(60, 20);
            this.neuronsBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Neurons in first layer:";
            // 
            // momentumBox
            // 
            this.momentumBox.Location = new System.Drawing.Point(125, 45);
            this.momentumBox.Name = "momentumBox";
            this.momentumBox.Size = new System.Drawing.Size(60, 20);
            this.momentumBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Momentum:";
            // 
            // alphaBox
            // 
            this.alphaBox.Location = new System.Drawing.Point(125, 70);
            this.alphaBox.Name = "alphaBox";
            this.alphaBox.Size = new System.Drawing.Size(60, 20);
            this.alphaBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sigmoid\'s alpha value:";
            // 
            // learningRateBox
            // 
            this.learningRateBox.Location = new System.Drawing.Point(125, 20);
            this.learningRateBox.Name = "learningRateBox";
            this.learningRateBox.Size = new System.Drawing.Size(60, 20);
            this.learningRateBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Learning rate:";
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(10, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(175, 2);
            this.label8.TabIndex = 22;
            // 
            // iterationsBox
            // 
            this.iterationsBox.Location = new System.Drawing.Point(125, 155);
            this.iterationsBox.Name = "iterationsBox";
            this.iterationsBox.Size = new System.Drawing.Size(60, 20);
            this.iterationsBox.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(126, 175);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 14);
            this.label10.TabIndex = 25;
            this.label10.Text = "( 0 - inifinity )";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(10, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 16);
            this.label9.TabIndex = 8;
            this.label9.Text = "Iterations:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.currentErrorBox);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.currentIterationBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(510, 210);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(195, 75);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Current iteration";
            // 
            // currentErrorBox
            // 
            this.currentErrorBox.Location = new System.Drawing.Point(125, 45);
            this.currentErrorBox.Name = "currentErrorBox";
            this.currentErrorBox.ReadOnly = true;
            this.currentErrorBox.Size = new System.Drawing.Size(60, 20);
            this.currentErrorBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Error:";
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(125, 20);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.ReadOnly = true;
            this.currentIterationBox.Size = new System.Drawing.Size(60, 20);
            this.currentIterationBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Iteration:";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(630, 305);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(540, 305);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(714, 338);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Approximation using Multi-Layer Neural Network";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        // On main form closing
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // check if worker thread is running
            if ((_workerThread != null) && (_workerThread.IsAlive))
            {
                _needToStop = true;
                _workerThread.Join();
            }
        }

        // Update settings controls
        private void UpdateSettings()
        {
            learningRateBox.Text = _learningRate.ToString();
            momentumBox.Text = _momentum.ToString();
            alphaBox.Text = _sigmoidAlphaValue.ToString();
            neuronsBox.Text = _neuronsInFirstLayer.ToString();
            iterationsBox.Text = _iterations.ToString();
        }

        protected void LoadData()
        {
            // show file selection dialog
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = null;
                // read maximum 50 points
                double[,] tempData = new double[50, 2];
                double minX = double.MaxValue;
                double maxX = double.MinValue;

                try
                {
                    // open selected file
                    reader = File.OpenText(openFileDialog.FileName);
                    string str = null;
                    int i = 0;

                    // read the data
                    while ((i < 50) && ((str = reader.ReadLine()) != null))
                    {
                        string[] strs = str.Split(';');
                        if (strs.Length == 1)
                            strs = str.Split(',');
                        // parse X
                        tempData[i, 0] = double.Parse(strs[0]);
                        tempData[i, 1] = double.Parse(strs[1]);

                        // search for min value
                        if (tempData[i, 0] < minX)
                            minX = tempData[i, 0];
                        // search for max value
                        if (tempData[i, 0] > maxX)
                            maxX = tempData[i, 0];

                        i++;
                    }

                    // allocate and set data
                    _data = new double[i, 2];
                    Array.Copy(tempData, 0, _data, 0, i * 2);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed reading the file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    // close file
                    if (reader != null)
                        reader.Close();
                }

                // update list and chart
                UpdateDataListView();
                chart.RangeX = new DoubleRange(minX, maxX);
                chart.UpdateDataSeries("data", _data);
                chart.UpdateDataSeries("solution", null);
                // enable "Start" button
                startButton.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the loadDataButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void loadDataButton_Click(object sender, System.EventArgs e)
        {
            LoadData();
        }

        // Update data in list view
        private void UpdateDataListView()
        {
            // remove all current records
            dataList.Items.Clear();
            // add new records
            for (int i = 0, n = _data.GetLength(0); i < n; i++)
            {
                dataList.Items.Add(_data[i, 0].ToString());
                dataList.Items[i].SubItems.Add(_data[i, 1].ToString());
            }
        }

        // Enable/disale controls
        private void EnableControls(bool enable)
        {
            loadDataButton.Enabled = enable;
            learningRateBox.Enabled = enable;
            momentumBox.Enabled = enable;
            alphaBox.Enabled = enable;
            neuronsBox.Enabled = enable;
            iterationsBox.Enabled = enable;

            startButton.Enabled = enable;
            stopButton.Enabled = !enable;
        }

        /// <summary>
        /// Calculates the solution based on learning rate, momentum and sigmoid's alpha value.
        /// </summary>
        private void CalculateSolution()
        {
            // clear previous solution
            for (int j = 0, n = _data.Length; j < n; j++)
            {
                if (dataList.Items[j].SubItems.Count > 1)
                    dataList.Items[j].SubItems.RemoveAt(1);
            }

            // get learning rate
            try
            {
                _learningRate = Math.Max(0.00001, Math.Min(1, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                _learningRate = 0.1;
            }
            // get momentum
            try
            {
                _momentum = Math.Max(0, Math.Min(0.5, double.Parse(momentumBox.Text)));
            }
            catch
            {
                _momentum = 0;
            }
            // get sigmoid's alpha value
            try
            {
                _sigmoidAlphaValue = Math.Max(0.001, Math.Min(50, double.Parse(alphaBox.Text)));
            }
            catch
            {
                _sigmoidAlphaValue = 2;
            }
            // iterations
            try
            {
                _iterations = Math.Max(0, int.Parse(iterationsBox.Text));
            }
            catch
            {
                _iterations = 1000;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            _needToStop = false;
            _workerThread = new Thread(new ThreadStart(SearchSolution));
            _workerThread.Start();
        }

        // On button "Start"
        private void startButton_Click(object sender, System.EventArgs e)
        {
            CalculateSolution();
        }

        // On button "Stop"
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // stop worker thread
            _needToStop = true;
            _workerThread.Join();
            _workerThread = null;
        }

        // Worker thread
        protected void SearchSolution()
        {
            // number of learning samples
            int samples = _data.GetLength(0);
            // data transformation factor
            double yFactor = 1.7 / chart.RangeY.Length;
            double yMin = chart.RangeY.Min;
            double xFactor = 2.0 / chart.RangeX.Length;
            double xMin = chart.RangeX.Min;

            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[1];
                output[i] = new double[1];

                // set input
                input[i][0] = (_data[i, 0] - xMin) * xFactor - 1.0;
                // set output
                output[i][0] = (_data[i, 1] - yMin) * yFactor - 0.85;
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
            double[,] solution = new double[50, 2];
            double[] networkInput = new double[1];

            // calculate X values to be used with solution function
            for (int j = 0; j < 50; j++)
            {
                solution[j, 0] = chart.RangeX.Min + (double)j * chart.RangeX.Length / 49;
            }

            // loop
            while (!_needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;

                // calculate solution
                for (int j = 0; j < 50; j++)
                {
                    networkInput[0] = (solution[j, 0] - xMin) * xFactor - 1.0;
                    solution[j, 1] = (network.Compute(networkInput)[0] + 0.85) / yFactor + yMin;
                }
                chart.UpdateDataSeries("solution", solution);
                // calculate error
                double learningError = 0.0;
                for (int j = 0, k = _data.GetLength(0); j < k; j++)
                {
                    networkInput[0] = input[j][0];
                    learningError += Math.Abs(_data[j, 1] - ((network.Compute(networkInput)[0] + 0.85) / yFactor + yMin));
                }

                // set current iteration's info
                //THREAD
                currentIterationBox.Text = iteration.ToString();
                currentErrorBox.Text = learningError.ToString("F3");

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((_iterations != 0) && (iteration > _iterations))
                    break;
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}

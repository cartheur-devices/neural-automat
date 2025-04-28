using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NeuralAlgorithm.ActivationFunctions;
using NeuralAlgorithm.Core.Learning;
using NeuralAlgorithm.Core.Networks;

namespace Automat
{
    public static class Solution
    {
        private static double[] _data = null;
        private static double[,] _dataToShow = null;
        private static double factor = 10;
        private static double yMin = 5;
        private static ListView dataListView;

        private static double _learningRate = 0.1;
        private static double _momentum = 0.1;
        private static double _sigmoidAlphaValue = 2.0;
        private static int _windowSize = 5;
        private static int _predictionSize = 1;
        private static int _iterations = 1000;

        private static Thread _workerThread;
        private static bool _needToStop;

        /// <summary>
        /// Calculates the solution based on learning rate, momentum and sigmoid's alpha value.
        /// </summary>
        public static void CalculateSolution()
        {
            // clear previous solution
            for (int j = 0, n = _data.Length; j < n; j++)
            {
                if (dataListView.Items[j].SubItems.Count > 1)
                    dataListView.Items[j].SubItems.RemoveAt(1);
            }

            // run worker thread
            _needToStop = false;
            //_workerThread = new Thread(new ThreadStart(SearchSolution));
            //_workerThread.Name = "SearchSolution Thread";

           // _workerThread.Start();
        }

        /// <summary>
        /// Searches the solution.
        /// </summary>
        public static double[,] SearchSolution(List<double[]> dataPiece, Hashtable datePositionTable, int bars)
        {
            // Decompile the list, want value at position [3].
            double dataParticulate;
            var dataParticulates = new[] { dataPiece[0][3] };

            for (var i = 0; i < bars; i++)
            {
                dataParticulates = new[] { dataPiece[i][3] };
                // STOPPED HERE. 11.04.2013.
            }
            var newCandle = new[] {Convert.ToDouble(bars)};
            //var _data = candlestick.ToArray();
            //_data = new double[candlestick.Count];
            //Array.Copy(newCandle, 0, _data, 0, candlestick.Count);

            // number of learning samples
            int samples = newCandle.Length - _predictionSize - _windowSize;
            // Trim the datetime and make an int as a representative table so that it can be found when plotting (sorting for the x-axis).
            //var _data = Convert.ToDouble(candlestick);
            //var _data = candlestick.ToArray();
            //var samples = _data.Length - _predictionSize - _windowSize;
            // data transformation factor
            //double factor = 1.7 / chart.RangeY.Length;
            //double yMin = chart.RangeY.Min;
            // prepare learning data
            double[][] input = new double[samples][];
            double[][] output = new double[samples][];

            for (int i = 0; i < samples; i++)
            {
                input[i] = new double[_windowSize];
                output[i] = new double[1];

                // set input
                for (int j = 0; j < _windowSize; j++)
                {
                    input[i][j] = (_data[i + j] - yMin) * factor - 0.85;//null
                }
                // set output
                output[i][0] = (_data[i + _windowSize] - yMin) * factor - 0.85;// date field is not a double....
            }

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(new BipolarSigmoidFunction(_sigmoidAlphaValue), _windowSize, _windowSize * 2, 1);

            // create teacher
            BackPropagationLearning teacher = new BackPropagationLearning(network);

            // set learning rate and momentum
            teacher.LearningRate = _learningRate;
            teacher.Momentum = _momentum;
            //network.Compute(input[samples]); Index outside bounds of array.

            // iterations
            int iteration = 1;

            // solution array
            int solutionSize = _data.Length - _windowSize;
            double[,] solution = new double[solutionSize, 2];
            double[] networkInput = new double[_windowSize];

            // calculate X values to be used with solution function
            for (int j = 0; j < solutionSize; j++)
            {
                solution[j, 0] = j + _windowSize;
            }

            // loop
            while (!_needToStop)
            {
                // run epoch of learning procedure
                double error = teacher.RunEpoch(input, output) / samples;
                int digits = 5;
                // calculate solution and learning and prediction errors
                double learningError = 0.0;
                double predictionError = 0.0;
                // go through all the data
                for (int i = 0, n = _data.Length - _windowSize; i < n; i++)
                {
                    // put values from current window as network's input
                    for (int j = 0; j < _windowSize; j++)
                    {
                        networkInput[j] = (_data[i + j] - yMin) * factor - 0.85;
                    }

                    // evaluate the function
                    solution[i, 1] = (network.Compute(networkInput)[0] + 0.85) / factor + yMin;//truncate the value returned here.

                    // truncate to five decimal places (one more than input)
                    Math.Round(solution[i, 1], digits);

                    // calculate prediction error
                    if (i >= n - _predictionSize)
                    {
                        predictionError += Math.Abs(solution[i, 1] - _data[_windowSize + i]);
                    }
                    else
                    {
                        learningError += Math.Abs(solution[i, 1] - _data[_windowSize + i]);
                    }

                }
                // update solution on the chart
                //chart.UpdateDataSeries("solution", solution);

                //Engage the background worker (optional)
                //SetBackgroundWorker();

                // set current cross-thread operation via the delegate
                //this.SetIteration(iteration);
                //this.SetLearningError(learningError);
                //this.SetPredictionError(predictionError);

                // increase current iteration
                iteration++;

                // check if we need to stop
                if ((_iterations != 0) && (iteration > _iterations))
                    break;
            }

            // show new solution via the delegate
            for (int j = _windowSize, k = 0, n = _data.Length; j < n; j++, k++)
            {
                //this.SetDataListView(dataListView, j, k, solution);//solution is already loaded here.
            }

            return solution;
        }

    }
}

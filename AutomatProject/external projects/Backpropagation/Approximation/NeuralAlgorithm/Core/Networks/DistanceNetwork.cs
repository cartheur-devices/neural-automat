using NeuralAlgorithm.Core.Layers;
using NeuralAlgorithm.Core.Neurons;

namespace NeuralAlgorithm.Core.Networks
{
    /// <summary>
    /// Distance network is a neural network of only one <see cref="DistanceLayer">distance
    /// layer</see>. The network is a base for such neural networks as SOM, Elastic net, etc.
    /// </summary>
    public class DistanceNetwork : Network
    {
        /// <summary>
        /// Network's layers accessor
        /// </summary>
        /// <value></value>
        /// <remarks>Allows to access network's layer.</remarks>
        public new DistanceLayer this[int index]
        {
            get { return ((DistanceLayer)layers[index]); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceNetwork"/> class
        /// </summary>
        /// <param name="inputsCount">Network's inputs count</param>
        /// <param name="neuronsCount">Network's neurons count</param>
        /// <remarks>The new network will be randomized (see <see cref="Neuron.Randomize"/>
        /// method) after it is created.</remarks>
        public DistanceNetwork(int inputsCount, int neuronsCount)
            : base(inputsCount, 1)
        {
            // create layer
            layers[0] = new DistanceLayer(neuronsCount, inputsCount);
        }

        /// <summary>
        /// Get winner neuron
        /// </summary>
        /// <returns>Index of the winner neuron</returns>
        /// <remarks>The method returns index of the neuron, which weights have
        /// the minimum distance from network's input.</remarks>
        public int GetWinner()
        {
            // find the MIN value
            double min = output[0];
            int minIndex = 0;

            for (int i = 1, n = output.Length; i < n; i++)
            {
                if (output[i] < min)
                {
                    // found new MIN value
                    min = output[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }
}

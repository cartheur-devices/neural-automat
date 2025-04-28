using NeuralAlgorithm.Core.Neurons;

namespace NeuralAlgorithm.Core.Layers
{
    /// <summary>
    /// Distance layer
    /// </summary>
    /// <remarks>Distance layer is a layer of <see cref="DistanceNeuron">distance neurons</see>.
    /// The layer is usually a single layer of such networks as Kohonen Self
    /// Organizing Map, Elastic Net, Hamming Memory Net.</remarks>
    public class DistanceLayer : Layer
    {
        /// <summary>
        /// Layer's neurons accessor
        /// </summary>
        /// <value></value>
        /// <remarks>Allows to access layer's neurons.</remarks>
        public new DistanceNeuron this[int index]
        {
            get { return (DistanceNeuron)neurons[index]; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceLayer"/> class
        /// </summary>
        /// <param name="neuronsCount">Layer's neurons count</param>
        /// <param name="inputsCount">Layer's inputs count</param>
        /// <remarks>The new layet will be randomized (see <see cref="Neuron.Randomize"/>
        /// method) after it is created.</remarks>
        public DistanceLayer(int neuronsCount, int inputsCount)
            : base(neuronsCount, inputsCount)
        {
            // create each neuron
            for (int i = 0; i < neuronsCount; i++)
                neurons[i] = new DistanceNeuron(inputsCount);
        }
    }
}

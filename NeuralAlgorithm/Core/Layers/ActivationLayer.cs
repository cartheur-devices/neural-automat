using NeuralAlgorithm.ActivationFunctions;
using NeuralAlgorithm.Core.Neurons;

namespace NeuralAlgorithm.Core.Layers
{
    /// <summary>
    /// Activation layer
    /// </summary>
    /// <remarks>Activation layer is a layer of <see cref="ActivationNeuron">activation neurons</see>.
    /// The layer is usually used in multi-layer neural networks.</remarks>
    public class ActivationLayer : Layer
    {
        /// <summary>
        /// Layer's neurons accessor
        /// </summary>
        /// <value></value>
        /// <remarks>Allows to access layer's neurons.</remarks>
        public new ActivationNeuron this[int index]
        {
            get { return (ActivationNeuron)neurons[index]; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationLayer"/> class
        /// </summary>
        /// <param name="neuronsCount">Layer's neurons count</param>
        /// <param name="inputsCount">Layer's inputs count</param>
        /// <param name="function">Activation function of neurons of the layer</param>
        /// <remarks>The new layet will be randomized (see <see cref="ActivationNeuron.Randomize"/>
        /// method) after it is created.</remarks>
        public ActivationLayer(int neuronsCount, int inputsCount, IActivationFunction function)
            : base(neuronsCount, inputsCount)
        {
            // create each neuron
            for (int i = 0; i < neuronsCount; i++)
                neurons[i] = new ActivationNeuron(inputsCount, function);
        }
    }
}

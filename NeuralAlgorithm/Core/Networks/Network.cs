using System;
using NeuralAlgorithm.Core.Layers;

namespace NeuralAlgorithm.Core.Networks
{
    /// <summary>
    /// This is a base neural netwok class, which represents a collection of neuron's layers.
    /// </summary>
    public abstract class Network
    {
        /// <summary>
        /// Network's inputs count
        /// </summary>
        protected int inputsCount;

        /// <summary>
        /// Network's layers count
        /// </summary>
        protected int layersCount;

        /// <summary>
        /// Network's layers
        /// </summary>
        protected Layer[] layers;

        /// <summary>
        /// Network's output vector
        /// </summary>
        protected double[] output;

        /// <summary>
        /// Network's inputs count
        /// </summary>
        /// <value>The inputs count.</value>
        public int InputsCount
        {
            get { return inputsCount; }
        }

        /// <summary>
        /// Network's layers count
        /// </summary>
        /// <value>The layers count.</value>
        public int LayersCount
        {
            get { return layersCount; }
        }

        /// <summary>
        /// Network's output vector
        /// </summary>
        /// <value>The output.</value>
        /// <remarks>The calculation way of network's output vector is determined by
        /// inherited class.</remarks>
        public double[] Output
        {
            get { return output; }
        }

        /// <summary>
        /// Network's layers accessor
        /// </summary>
        /// <value></value>
        /// <remarks>Allows to access network's layer.</remarks>
        public Layer this[int index]
        {
            get { return layers[index]; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class
        /// </summary>
        /// <param name="inputsCount">Network's inputs count</param>
        /// <param name="layersCount">Network's layers count</param>
        /// <remarks>Protected constructor, which initializes <see cref="inputsCount"/>,
        /// <see cref="layersCount"/> and <see cref="layers"/> members.</remarks>
        protected Network(int inputsCount, int layersCount)
        {
            this.inputsCount = Math.Max(1, inputsCount);
            this.layersCount = Math.Max(1, layersCount);
            // create collection of layers
            layers = new Layer[this.layersCount];
        }

        /// <summary>
        /// Compute output vector of the network
        /// </summary>
        /// <param name="input">Input vector</param>
        /// <returns>Returns network's output vector</returns>
        /// <remarks>The actual network's output vecor is determined by inherited class and it
        /// represents an output vector of the last layer of the network. The output vector is
        /// also stored in <see cref="Output"/> property.</remarks>
        public virtual double[] Compute(double[] input)
        {
            output = input;

            // compute each layer
            foreach (Layer layer in layers)
            {
                output = layer.Compute(output);
            }

            return output;
        }

        /// <summary>
        /// Randomize layers of the network
        /// </summary>
        /// <remarks>Randomizes network's layers by calling <see cref="Layer.Randomize"/> method
        /// of each layer.</remarks>
        public virtual void Randomize()
        {
            foreach (Layer layer in layers)
            {
                layer.Randomize();
            }
        }
    }
}

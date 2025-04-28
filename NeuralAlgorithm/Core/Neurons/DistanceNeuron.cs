using System;

namespace NeuralAlgorithm.Core.Neurons
{
    /// <summary>
    /// Distance neuron
    /// </summary>
    /// <remarks>Distance neuron computes its output as distance between
    /// its weights and inputs. The neuron is usually used in Kohonen
    /// Self Organizing Map.</remarks>
    public class DistanceNeuron : Neuron
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceNeuron"/> class
        /// </summary>
        /// <param name="inputs">Neuron's inputs count</param>
        /// <remarks>The new neuron will be randomized (see <see cref="Randomize"/> method)
        /// after it is created.</remarks>
        public DistanceNeuron(int inputs) : base(inputs) { }


        /// <summary>
        /// Computes output value of neuron
        /// </summary>
        /// <param name="input">Input vector</param>
        /// <returns>
        /// The output value of distance neuron is equal to distance
        /// between its weights and inputs - sum of absolute differences.
        /// The output value is also stored in <see cref="Neuron.Output">Output</see>
        /// property.
        /// </returns>
        /// <remarks>The actual neuron's output value is determined by inherited class.
        /// The output value is also stored in <see cref="Output"/> property.</remarks>
        public override double Compute(double[] input)
        {
            output = 0.0;

            // compute distance between inputs and weights
            for (int i = 0; i < inputsCount; i++)
            {
                output += Math.Abs(weights[i] - input[i]);
            }
            return output;
        }
    }
}

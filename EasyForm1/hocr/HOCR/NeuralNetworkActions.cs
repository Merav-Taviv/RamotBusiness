/*
* This file is Part of a HOCR
* LGPLv3 Licence:
*       Copyright (c) 2011 
*          Niv Maman [nivmaman@yahoo.com]
*          Maxim Drabkin [mdrabkin@gmail.com]
*          Hananel Hazan [hhazan01@CS.haifa.ac.il]
*          University of Haifa
* This Project is part of our B.Sc. Project course that under supervision
* of Hananel Hazan [hhazan01@CS.haifa.ac.il]
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without 
* modification, are permitted provided that the following conditions are met:
*
* 1. Redistributions of source code must retain the above copyright notice, this list of conditions 
*    and the following disclaimer.
* 2. Redistributions in binary form must reproduce the above copyright notice, this list of 
*    conditions and the following disclaimer in the documentation and/or other materials provided
*    with the distribution.
* 3. Neither the name of the <ORGANIZATION> nor the names of its contributors may be used to endorse
*    or promote products derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
* ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
* DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
* SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
* CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
* LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
* OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
* DAMAGE.
*/

using System;
using System.Collections.Generic;
using Encog.Engine.Network.Activation;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.NeuralData;

namespace HOCR
{
    /// <summary>
    /// Class that contain data of neural network and his dataset.
    /// includes actions of creation, training, computing and more.
    /// </summary>
    [Serializable]
    public class NeuralNetwork
    {
        private readonly BasicNetwork _network; //neural network
        private readonly INeuralDataSet _dataSet; //network data
        private bool _isActive; //is network active
        public event EventHandler<TrainArgs> IterationChanged; //event that raised every iteration

        /// <summary>
        /// C'tor. create the network and load dataset
        /// </summary>
        /// <param name="inputLayer">number of neuron in input layer</param>
        /// <param name="middleLayers">array of number of neuron in middle layer</param>
        /// <param name="outputLayer">number of neuron in output layer</param>
        /// <param name="inputData">array of vector represents input data</param>
        /// <param name="outputData">array of vector represents output data</param>
        public NeuralNetwork(int inputLayer, IEnumerable<int> middleLayers, int outputLayer,
            double[][] inputData, double[][] outputData)
        {
            _network = new BasicNetwork();
            _network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, inputLayer));
            foreach (var layer in middleLayers)
                _network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, layer));

            _network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, outputLayer));
            _network.Structure.FinalizeStructure();
            _network.Reset();
            _dataSet = new BasicNeuralDataSet(inputData, outputData);
        }

        /// <summary>
        /// Train the neural network on the dataset
        /// </summary>
        /// <param name="repeats">number of repeats</param>
        /// <param name="error">error to stop</param>
        /// <param name="learnRate">learn rate</param>
        /// <param name="momentum">momentum</param>
        public void Train(int repeats = 10000, double error = 0.001, double learnRate=0.1, double momentum=0.1)
        {
            //var train = new Encog.Neural.Networks.Training.Propagation.Resilient.ResilientPropagation(_network, _dataSet);
            var train = new Backpropagation(_network, _dataSet, learnRate, momentum);
            _isActive = true;
            var epoch = 1;
            do
            {
                train.Iteration();
                epoch++;
                IterationChanged.Invoke(null,new TrainArgs{Error = train.Error,Iterations = epoch});
            } while ((epoch < repeats) && (train.Error > error) && _isActive);
        }

        /// <summary>
        /// Stop training
        /// </summary>
        public void StopTrain()
        {
            _isActive = false;
        }

        /// <summary>
        /// Get input, compute output and return it.
        /// </summary>
        /// <param name="letter">input sample</param>
        /// <returns>output result</returns>
        public double[] Compute(double [] letter)
        {
            var input = new BasicNeuralData(letter);
            var output = _network.Compute(input);
            return output.Data;
        }

        /// <summary>
        /// Test the network and return a string
        /// of all the samples with the network result
        /// </summary>
        /// <returns>string of result of all samples</returns>
        public string Test()
        {
            var message = "";
            foreach (var pair in _dataSet)
            {
                var output = _network.Compute(pair.Input);
                var outputMessage = "";
                var idealMessage = "";
                for (var i = 0; i < output.Count; i++)
                {
                    idealMessage += string.Format("{0},", pair.Ideal[i]);
                    outputMessage += string.Format("{0:0.#},", output[i]);
                }
                message += string.Format("Ideal   =" + idealMessage + Environment.NewLine
                                       + "Actual  =" + outputMessage + Environment.NewLine);
            }
            return message;
        }

        /// <summary>
        /// Get new letter data and add it to data set
        /// </summary>
        /// <param name="data">new letter data</param>
        public void AddToDataSet(double[][] data)
        {
            var input = new BasicNeuralData(data[0]); //letterSize*letterSize
            var output = new BasicNeuralData(data[1]); //numberOfLetters
            _dataSet.Add(input, output);
        }
    }

    /// <summary>
    /// Class that holds event arguments: iterations and error
    /// </summary>
    public class TrainArgs : EventArgs
    {
        public int Iterations { get; set; }
        public double Error { get; set; }
    }
}
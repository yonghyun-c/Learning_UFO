using System;
using System.Collections.Generic;
using System.Linq;

public interface INeuron
{
    double GetOutput();
    void UpdateWeight(double error);
    void CopyWeight(INeuron neuron);
    List<double> GetWeight();
    List<INeuron> GetInputs();
    string ToString();
}

public class InputNeuron : INeuron
{
    private static readonly List<double> EMPTY_WEIGHT_LIST = new List<double>();
    private static readonly List<INeuron> EMPTY_INPUTS_LIST = new List<INeuron>();

    private Player player;
    private readonly int idx;

    public InputNeuron(Player player, int idx)
    {
        this.player = player;
        this.idx = idx;
    }

    public void UpdateInput(Player newPlayer)
    {
        player = newPlayer;
    }

    public double GetOutput()
    {
        return player.GetSensorInput(idx);
    }

    public List<double> GetWeight()
    {
        return EMPTY_WEIGHT_LIST;
    }

    public List<INeuron> GetInputs()
    {
        return EMPTY_INPUTS_LIST;
    }

    public void UpdateWeight(double error)
    {
        // Nothing
    }

    public void CopyWeight(INeuron neuron)
    {
        // Nothing
    }

    public override string ToString()
    {
        return GetOutput().ToString();
    }
}

public abstract class Neuron: INeuron
{
    protected readonly Random rnd;
    protected readonly double threshold;

    protected List<INeuron> inputs;
    protected List<double> weights;

    public Neuron(params INeuron[] neurons)
    {
        rnd = new Random(this.GetHashCode());

        threshold = 1;
        this.inputs = new List<INeuron>();
        this.weights = new List<double>();

        SetInputNeurons(neurons);
    }

    private void SetInputNeurons(params INeuron[] neurons)
    {
        foreach (INeuron neuron in neurons)
        {
            this.inputs.Add(neuron);
            this.weights.Add(rnd.NextDouble() * 10 - 5);
        }
    }
    
    public List<double> GetWeight()
    {
        return weights;
    }

    public List<INeuron> GetInputs()
    {
        return inputs;
    }

    public abstract double GetOutput();

    public void UpdateWeight(double error)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            double change = rnd.NextDouble() * (error * 2) - error;
            weights[i] += change;
            inputs[i].UpdateWeight(change * 0.4);
        }
    }

    public void CopyWeight(INeuron neuron)
    {
        weights = new List<double>(neuron.GetWeight());

        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].CopyWeight(neuron.GetInputs()[i]);
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach (double weight in weights)
        {
            result = result + Math.Round(weight, 2).ToString() + ", ";
        }

        return result;
    }
}

public class HiddenNeuron: Neuron
{
    public HiddenNeuron(params INeuron[] neurons) : base(neurons)
    {

    }

    public override double GetOutput()
    {
        double sum = 0f;

        for (int i = 0; i < inputs.Count; i++)
        {
            sum += inputs[i].GetOutput() * weights[i];
        }

        return (sum >= threshold) ? 1 : 0;
    }
}

public class OutputNeuron: Neuron
{
    public OutputNeuron(params INeuron[] neurons): base(neurons)
    {
        
    }

    public override double GetOutput()
    {
        double sum = 0f;

        for (int i = 0; i < inputs.Count; i++)
        {
            sum += inputs[i].GetOutput() * weights[i];
        }

        return sum;
    }
}
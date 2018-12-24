using System;
using System.Collections.Generic;
using System.Linq;

public interface INeuron
{
    double getOutput();
    void UpdateWeight(double error);
    void CopyWeight(INeuron neuron);
    List<double> GetWeight();
    List<INeuron> GetInputs();
    string ToString();
}

public class HiddenNeuron: INeuron
{
    protected Random rnd;
    protected double threshold;

    protected List<INeuron> inputs;
    protected List<double> weights;

    public static HiddenNeuron Create(params INeuron[] neurons)
    {
        HiddenNeuron newNeuron = new HiddenNeuron();

        foreach (INeuron neuron in neurons)
        {
            newNeuron.AddInput(neuron);
        }

        return newNeuron;
    }

    protected HiddenNeuron()
    {
        rnd = new Random(this.GetHashCode());

        threshold = 1;
        this.inputs = new List<INeuron>();
        this.weights = new List<double>();
    }

    protected void AddInput(INeuron input)
    {
        this.inputs.Add(input);
        this.weights.Add(rnd.NextDouble() * 10 - 5);
    }

    public List<double> GetWeight()
    {
        return weights;
    }

    public List<INeuron> GetInputs()
    {
        return inputs;
    }

    public double getOutput()
    {
       double sum = 0f;

       for (int i=0; i<inputs.Count; i++)
        {
            sum += inputs[i].getOutput() * weights[i];
        }

       if (sum >= threshold)
        {
            return 1;
        }

       return 0;
    }

    public void UpdateWeight(double error)
    {
        for (int i=0; i<inputs.Count; i++)
        {
            double change = rnd.NextDouble() * (error*2) - error;
            weights[i] += change;
            inputs[i].UpdateWeight(change * 0.4);
        }
    }

    public void CopyWeight(INeuron neuron)
    {
        weights = new List<double>(neuron.GetWeight());

        for(int i=0; i< inputs.Count; i++)
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

public class OutputNeuron : HiddenNeuron
{
    public static OutputNeuron Create(params INeuron[] neurons)
    {
        OutputNeuron newNeuron = new OutputNeuron();

        foreach (INeuron neuron in neurons)
        {
            newNeuron.AddInput(neuron);
        }

        return newNeuron;
    }

    public double getOutput()
    {
        double sum = 0f;

        for (int i = 0; i < inputs.Count; i++)
        {
            sum += inputs[i].getOutput() * weights[i];
        }

        return sum;
    }
}

public class InputNeuron: INeuron
{
    private Player player;
    private int idx;

    private static readonly List<double> EMPTY_WEIGHT_LIST = new List<double>();
    private static readonly List<INeuron> EMPTY_INPUTS_LIST = new List<INeuron>();

    public static InputNeuron Create(Player player, int idx)
    {
        InputNeuron newNeuron = new InputNeuron();
        newNeuron.player = player;
        newNeuron.idx = idx;

        return newNeuron;
    }

    public void UpdateInput(Player newPlayer)
    {
        player = newPlayer;
    }

    public double getOutput()
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
        return getOutput().ToString();
    }
}
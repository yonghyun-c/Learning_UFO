using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NeuronStructure
{
    Player player;

    private InputNeuron[] inputNeurons;
    private HiddenNeuron[][] hiddenNeurons;
    private OutputNeuron[] outputNeurons;

    List<int> ancestors = new List<int>();

    public NeuronStructure(Player inputPlayer, params int[] neuronNumers)
    {
        player = inputPlayer;
        CreateLayer(neuronNumers);
    }

    public void CreateLayer(params int[] neuronNumers)
    {
        if (neuronNumers.Length < 2)
        {
            throw new FormatException("Neuron Structure should include at leat input, output nueorn layer. CURRENT: " + neuronNumers.Length);
        }

        inputNeurons = new InputNeuron[neuronNumers[0]];
        for (int i = 0; i < inputNeurons.Length; i++)
        {
            inputNeurons[i] = InputNeuron.Create(player, i);
        }

        hiddenNeurons = new HiddenNeuron[neuronNumers.Length - 2][];
        for (int i = 0; i < hiddenNeurons.Length; i++)
        {
            hiddenNeurons[i] = new HiddenNeuron[neuronNumers[i + 1]];

            INeuron[] prevNeurons = (i == 0) ? (INeuron[])inputNeurons : (INeuron[])hiddenNeurons[i - 1];
            for (int j = 0; j < hiddenNeurons[i].Length; j++)
            {
                hiddenNeurons[i][j] = HiddenNeuron.Create(prevNeurons);
            }
        }

        outputNeurons = new OutputNeuron[neuronNumers[neuronNumers.Length - 1]];
        {
            INeuron[] prevNeurons = (neuronNumers.Length == 2) ? (INeuron[])inputNeurons : (INeuron[])hiddenNeurons[hiddenNeurons.Length - 1];
            for (int i = 0; i < outputNeurons.Length; i++)
            {
                outputNeurons[i] = OutputNeuron.Create(prevNeurons);
            }
        }
    }

    public float GetOuput(int idx)
    {
        return (float)outputNeurons[idx].getOutput();
    }

    public void UpdateWeight()
    {
        for (int i=0; i<outputNeurons.Length; i++)
        {
            outputNeurons[i].UpdateWeight(player.GetError());
        }
    }

    public void CopyWeight(NeuronStructure neuronStructure)
    {
        for (int i = 0; i < outputNeurons.Length; i++)
        {
            outputNeurons[i].CopyWeight(neuronStructure.outputNeurons[i]);
        }

        ancestors.Add(neuronStructure.getId());
    }

    public void UpdateInput(Player player)
    {
        for (int i = 0; i < inputNeurons.Length; i++)
        {
            inputNeurons[i].UpdateInput(player);
        }
    }

    public double GetPriority()
    {
        return -1 * player.GetError();
    }

    public int getId()
    {
        return GetHashCode();
    }
}

public class NeuronStructureFactory
{
    private static readonly int SIZE = 45;
    private static readonly int PART = 5;
    private static readonly int[] NEURON_NUMBERS = { 5, 6, 6, 2 };

    private static int step = 0;
    private static NeuronStructure[] structures = new NeuronStructure[SIZE];
    private static int index = 0;

    public static NeuronStructure CreateStructure(Player player)
    {
        lock (structures)
        {
            index = (index + 1) % SIZE;

            if (structures[index] == null)
            {
                structures[index] = new NeuronStructure(player, NEURON_NUMBERS);
            }
            else
            {
                structures[index].UpdateInput(player);
            }
        }

        return structures[index];
    }

    public static void MoveToNextStep()
    {
        step++;

        Array.Sort(structures, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));

        NeuronStructure temp = structures[structures.Length - 4];
        structures[structures.Length - 4] = structures[structures.Length / 2];
        structures[structures.Length / 2] = temp;

        temp = structures[structures.Length - 5];
        structures[structures.Length - 5] = structures[0];
        structures[0] = temp;

        int from = 0;
        int to = from + structures.Length / PART;

        for (int j = 1; j < PART; j++)
        {
            for (int i = from; i < to; i++)
            {
                structures[i].CopyWeight(structures[structures.Length - j]);
                structures[i].UpdateWeight();
            }

            from = to;
            to = to + structures.Length / PART;
        }

        for (int i = from; i < structures.Length - 1; i++)
        {
            structures[i].CopyWeight(structures[structures.Length - PART]);
            structures[i].UpdateWeight();
        }
    }

    public static int GetStep()
    {
        return step;
    }
}
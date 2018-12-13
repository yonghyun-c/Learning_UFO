using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NeuronStructure
{
    private static readonly int PICK_UP_COUNT = 12;

    InputNeuron input_neuron_1;
    InputNeuron input_neuron_2;
    InputNeuron input_neuron_3;
    InputNeuron input_neuron_4;
    InputNeuron input_neuron_5;

    HiddenNeuron hidden_1_1;
    HiddenNeuron hidden_1_2;
    HiddenNeuron hidden_1_3;
    HiddenNeuron hidden_1_4;
    HiddenNeuron hidden_1_5;
    HiddenNeuron hidden_1_6;


    HiddenNeuron hidden_2_1;
    HiddenNeuron hidden_2_2;
    HiddenNeuron hidden_2_3;
    HiddenNeuron hidden_2_4;
    HiddenNeuron hidden_2_5;
    HiddenNeuron hidden_2_6;

    OutputNeuron direction_neuron;
    OutputNeuron speed_neuron;

    bool winner;
    int count;
    int endTime = 9876;
    int id;

    List<int> ancestors = new List<int>();

    public NeuronStructure(Dictionary<string, SensorController> sensors, int index)
    {
        winner = false;
        count = 0;
        id = index;

        input_neuron_1 = InputNeuron.Create(sensors["Sensor_1"]);
        input_neuron_2 = InputNeuron.Create(sensors["Sensor_2"]);
        input_neuron_3 = InputNeuron.Create(sensors["Sensor_3"]);
        input_neuron_4 = InputNeuron.Create(sensors["Sensor_4"]);
        input_neuron_5 = InputNeuron.Create(sensors["Sensor_5"]);

        hidden_1_1 = HiddenNeuron.Create(input_neuron_1, input_neuron_2, input_neuron_3, input_neuron_4, input_neuron_5);
        hidden_1_2 = HiddenNeuron.Create(input_neuron_1, input_neuron_2, input_neuron_3, input_neuron_4, input_neuron_5);
        hidden_1_3 = HiddenNeuron.Create(input_neuron_1, input_neuron_2, input_neuron_3, input_neuron_4, input_neuron_5);
        hidden_1_4 = HiddenNeuron.Create(input_neuron_1, input_neuron_2, input_neuron_3, input_neuron_4, input_neuron_5);
        hidden_1_5 = HiddenNeuron.Create(input_neuron_1, input_neuron_2, input_neuron_3, input_neuron_4, input_neuron_5);
        hidden_1_6 = HiddenNeuron.Create(input_neuron_1, input_neuron_2, input_neuron_3, input_neuron_4, input_neuron_5);

        hidden_2_1 = HiddenNeuron.Create(hidden_1_1, hidden_1_2, hidden_1_3, hidden_1_4, hidden_1_5, hidden_1_6);
        hidden_2_2 = HiddenNeuron.Create(hidden_1_1, hidden_1_2, hidden_1_3, hidden_1_4, hidden_1_5, hidden_1_6);
        hidden_2_3 = HiddenNeuron.Create(hidden_1_1, hidden_1_2, hidden_1_3, hidden_1_4, hidden_1_5, hidden_1_6);
        hidden_2_4 = HiddenNeuron.Create(hidden_1_1, hidden_1_2, hidden_1_3, hidden_1_4, hidden_1_5, hidden_1_6);
        hidden_2_5 = HiddenNeuron.Create(hidden_1_1, hidden_1_2, hidden_1_3, hidden_1_4, hidden_1_5, hidden_1_6);
        hidden_2_6 = HiddenNeuron.Create(hidden_1_1, hidden_1_2, hidden_1_3, hidden_1_4, hidden_1_5, hidden_1_6);

        direction_neuron = OutputNeuron.Create(hidden_2_1, hidden_2_2, hidden_2_3, hidden_2_4, hidden_2_5, hidden_2_6);
        speed_neuron = OutputNeuron.Create(hidden_2_1, hidden_2_2, hidden_2_3, hidden_2_4, hidden_2_5, hidden_2_6);
    }

    public float GetDirection()
    {
        return (float)direction_neuron.getOutput();
    }

    public float GetSpeed()
    {
        return (float)speed_neuron.getOutput();
    }

    public void UpdateWeight(int count)
    {
        double error = (12 - count) / 2.0;
        error = Math.Max(error, 0.5);

        direction_neuron.UpdateWeight(error);
        speed_neuron.UpdateWeight(error);
    }

    public void CopyWeight(NeuronStructure neuronStructure)
    {
        this.direction_neuron.CopyWeight(neuronStructure.direction_neuron);
        this.speed_neuron.CopyWeight(neuronStructure.speed_neuron);

        ancestors.Add(neuronStructure.getId());
    }

    public void Reset()
    {
        ResetWinner();
        count = 0;
    }

    public void UpdateInput(Dictionary<string, SensorController> sensors)
    {
        input_neuron_1.UpdateInput(sensors["Sensor_1"]);
        input_neuron_2.UpdateInput(sensors["Sensor_2"]);
        input_neuron_3.UpdateInput(sensors["Sensor_3"]);
        input_neuron_4.UpdateInput(sensors["Sensor_4"]);
        input_neuron_5.UpdateInput(sensors["Sensor_5"]);
    }

    public bool IsWinner()
    {
        return winner;
    }

    public void SetWinner()
    {
        winner = true;
    }

    public void ResetWinner()
    {
        winner = false;
    }

    public string getNeuronStructure()
    {
        string result = "";
        result = result + input_neuron_1.ToString() + "\n";
        result = result + input_neuron_2.ToString() + "\n";
        result = result + input_neuron_3.ToString() + "\n";
        result = result + input_neuron_4.ToString() + "\n";
        result = result + "-------------\n";

        result = result + hidden_1_1.ToString() + "\n";
        result = result + hidden_1_2.ToString() + "\n";
        result = result + hidden_1_3.ToString() + "\n";
        result = result + hidden_1_4.ToString() + "\n";
        result = result + hidden_1_5.ToString() + "\n";
        result = result + hidden_1_6.ToString() + "\n";
        result = result + "-------------\n";

        result = result + hidden_2_1.ToString() + "\n";
        result = result + hidden_2_2.ToString() + "\n";
        result = result + hidden_2_3.ToString() + "\n";
        result = result + hidden_2_4.ToString() + "\n";
        result = result + hidden_2_5.ToString() + "\n";
        result = result + hidden_2_6.ToString() + "\n";
        result = result + "-------------\n";

        result = result + direction_neuron.ToString() + "\n";
        result = result + speed_neuron.ToString() + "\n";
        result = result + "-------------\n";

        return result;
    }

    public void IncreaseCount(int timer)
    {
        count++;

        if (count >= PICK_UP_COUNT)
        {
            endTime = timer;
        }
    }

    public int GetCount()
    {
        return count;
    }

    public int GetPriority()
    {
        return count * 10000 - endTime;
    }

    public int getId()
    {
        return id;
    }
}

public class NeuronStructureFactory
{
    private static readonly int SIZE = 45;
    private static readonly int PART = 5;

    private static int step = 0;
    private static NeuronStructure[] structures = new NeuronStructure[SIZE];
    private static int index = 0;

    public static NeuronStructure CreateStructure(Dictionary<string, SensorController> sensors)
    {
        lock (structures)
        {
            index = (index + 1) % SIZE;

            if (structures[index] == null)
            {
                structures[index] = new NeuronStructure(sensors, index);
            }
            else
            {
                structures[index].UpdateInput(sensors);
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

        for (int j=1; j<PART; j++)
        {
            for (int i = from; i < to; i++)
            {
                structures[i].CopyWeight(structures[structures.Length - j]);
                structures[i].UpdateWeight(structures[structures.Length - j].GetCount());
                structures[i].Reset();
            }

            from = to;
            to = to + structures.Length / PART;
        }

        for (int i = from; i < structures.Length - 1; i++)
        {
            structures[i].CopyWeight(structures[structures.Length - PART]);
            structures[i].UpdateWeight(structures[structures.Length - PART].GetCount());
            structures[i].Reset();
        }

        structures[structures.Length - 1].Reset();
        structures[structures.Length - 1].SetWinner();
    }

    public static int GetStep()
    {
        return step;
    }
}
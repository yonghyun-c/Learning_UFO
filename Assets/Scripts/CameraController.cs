using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Text timeText;
    private int time;
    private int step;

    public Text stepText;

    private void Start()
    {
        time = 0;
        timeText.text = "";
        step = NeuronStructureFactory.GetStep();
        stepText.text = "Step: " + step;
    }

    private void Update()
    {
        time++;
        timeText.text = "Time: " + time.ToString();

        if (time > (2500 + step * 100))
        {
            MoveToNextStep();
        }
    }

    private void MoveToNextStep()
    {
        NeuronStructureFactory.MoveToNextStep();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetTimer()
    {
        return time;
    }
}
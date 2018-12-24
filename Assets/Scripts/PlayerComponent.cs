using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour, Player {
    private static readonly int SPEED = 0, DIRECTION = 1;

    public SpriteRenderer winner;

    private Rigidbody2D rb2d;

    private NeuronStructure ics;

    private PlayerColliderComponent playerColliderComponent;

    private List<SensorController> sensors;

    private CameraController m_MainCameraComponent;

    private int count = 0;

    private void Start()
    {
        m_MainCameraComponent = Camera.allCameras[0].GetComponent<CameraController>();
        rb2d = GetComponent<Rigidbody2D>();

        sensors = new List<SensorController>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Sensor"))
            {
                sensors.Add(child.gameObject.GetComponent<SensorController>());
            }
            if (child.gameObject.CompareTag("PickUpSensor"))
            {
                playerColliderComponent = child.gameObject.GetComponent<PlayerColliderComponent>();
            }
        }

        ics = NeuronStructureFactory.CreateStructure(this);
    }

    private void Update()
    {
        rb2d.velocity = transform.up * ics.GetOuput(SPEED);
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * ics.GetOuput(DIRECTION), Space.World);
    }

    private void ResetGame()
    {
        ics.UpdateWeight();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PickItem()
    {
        foreach(SensorController sensor in sensors)
        {
            sensor.ReleasePickup();
        }

        count++;
    }

    public void HitTheWall()
    {
        //ResetGame();
        rb2d.Sleep();
    }

    public double GetSensorInput(int idx)
    {
        return sensors[idx].getDistance();
    }

    public double GetError()
    {
        double error = (12 - count) / 2.0;
        return Math.Max(error, 0.5);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour {

    public SpriteRenderer winner;

    private Rigidbody2D rb2d;

    private NeuronStructure ics;

    private PlayerColliderComponent playerColliderComponent;

    private Dictionary<string, SensorController> sensors;

    private CameraController m_MainCameraComponent;

    private void Start()
    {
        m_MainCameraComponent = Camera.allCameras[0].GetComponent<CameraController>();
        rb2d = GetComponent<Rigidbody2D>();

        sensors = new Dictionary<string, SensorController>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Sensor"))
            {
                sensors.Add(child.gameObject.name, child.gameObject.GetComponent<SensorController>());
            }
            if (child.gameObject.CompareTag("PickUpSensor"))
            {
                playerColliderComponent = child.gameObject.GetComponent<PlayerColliderComponent>();
            }
        }

        ics = NeuronStructureFactory.CreateStructure(sensors);

        winner.enabled = ics.IsWinner();
    }

    private void Update()
    {
        rb2d.velocity = transform.up * ics.GetSpeed();
        transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * ics.GetDirection(), Space.World);
    }

    private void ResetGame()
    {
        ics.UpdateWeight(playerColliderComponent.getCount());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PickItem()
    {
        foreach(KeyValuePair<string, SensorController> keyvalue in sensors)
        {
            keyvalue.Value.ReleasePickup();
        }

        ics.IncreaseCount(m_MainCameraComponent.GetTimer());
    }

    public void HitTheWall()
    {
        //ResetGame();
        rb2d.Sleep();
    }
}

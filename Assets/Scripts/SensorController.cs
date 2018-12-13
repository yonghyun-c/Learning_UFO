using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour {

    private static readonly Color DETECT_COLOR = new Color(155f, 0f, 0f, .4f);
    private static readonly Color RELEASE_COLOR = new Color(155f, 155f, 155f, .4f);

    float distance = 987654321;

    SpriteRenderer m_SpriteRenderer;
    Collider2D nearbyPickUp;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = RELEASE_COLOR;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            SetDistance(other.transform.position);
            nearbyPickUp = other;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            nearbyPickUp = null;
        }
    }

    public void SetDistance(Vector3 position)
    {
        distance = Vector3.Distance(transform.position, position);
        m_SpriteRenderer.color = DETECT_COLOR;
    }

    public void ReleasePickup()
    {
        distance = 100;
        m_SpriteRenderer.color = RELEASE_COLOR;
    }

    private void Update()
    {
        //transform.rotation = Quaternion.identity;
        if (nearbyPickUp == null)
        {
            ReleasePickup();
            return;
        }

        float currentDistance = Vector3.Distance(transform.position, nearbyPickUp.transform.position);

        if (currentDistance > distance + 0.1f)
        {
            ReleasePickup();
        }
    }

    public float getDistance()
    {
        return distance;
    }
}

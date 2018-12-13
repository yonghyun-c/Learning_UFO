using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColliderComponent : MonoBehaviour {

    private int count;

    private void Start()
    {
        count = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerComponent playerComponent = transform.parent.gameObject.GetComponent<PlayerComponent>();

        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;

            playerComponent.PickItem();
        }

        if (other.gameObject.CompareTag("BackgroundBorder"))
        {
            playerComponent.HitTheWall();
        }
    }

    public int getCount()
    {
        return count;
    }
}

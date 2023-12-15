using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Lila Masand
public class MoveWithPlatform : MonoBehaviour
{
    public GameObject player;
    private bool PlayerOn;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // If the platform isn't coming down on the player's head, parent them 
            if (transform.position.y < other.transform.gameObject.transform.position.y)
            {
                player.transform.parent = this.transform;
                player.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.transform.parent = null;
        }
    }  
}

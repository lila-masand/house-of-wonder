using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Lila Masand
public class respawn : MonoBehaviour
{
    public GameObject player;
    public GameObject respawn_point;

    // Respawn player at given checkpoint
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.transform.position = respawn_point.transform.position;
        }

    }
}
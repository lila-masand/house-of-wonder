using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by Lila Masand
public class ChangeCheckpoint : MonoBehaviour
{
    public GameObject check;
    public GameObject respawnNet;
    private respawn respawnScript;

    void Start()
    {
        respawnScript = respawnNet.GetComponent<respawn>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // If the player is actually on top of the object, update the checkpoint
            if(GetComponent<Collider>().bounds.max.y <= other.bounds.min.y + 0.1f)
                respawnScript.respawn_point = check;
        }
    }
}

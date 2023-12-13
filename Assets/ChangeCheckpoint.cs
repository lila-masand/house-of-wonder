using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCheckpoint : MonoBehaviour
{


    public GameObject check;
    public GameObject respawnNet;
    public respawn respawnScript;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            respawnScript.respawn_point = check;
        }

    }

}

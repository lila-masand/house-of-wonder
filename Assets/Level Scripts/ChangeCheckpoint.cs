using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if(GetComponent<Collider>().bounds.max.y <= other.bounds.min.y + 0.1f)
                respawnScript.respawn_point = check;

        }

    }

}

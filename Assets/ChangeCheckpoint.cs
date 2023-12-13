using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCheckpoint : MonoBehaviour
{


    public GameObject check;
    public GameObject respawnNet;
    public respawn respawnScript;
    
    // Start is called before the first frame update
    void Start()
    {

        respawnScript = respawnNet.GetComponent<respawn>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            respawnScript.respawn_point = check;
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    public GameObject player;
    public GameObject respawn_point;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // need overall level script that has a variable tracking Thyra's last position?

            player.transform.position = respawn_point.transform.position;

        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    public GameObject player;

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
        if (other.gameObject.name == "thyra" || other.gameObject.tag == "Player")
        {
            // need overall level script that has a variable tracking Thyra's last position?

            player.transform.position = new Vector3(38f, 46f, 371f);

        }

    }
}
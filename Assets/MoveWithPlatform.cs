using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlatform : MonoBehaviour
{
    public GameObject player;
    private bool PlayerOn;

    // Start is called before the first frame update
    void Start()
    {

        PlayerOn = false;
    }

    // Update is called once per frame
    void Update()
    {

       

    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOn = true;
            player.GetComponent<Rigidbody>().isKinematic = true;

            player.transform.parent = this.transform;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // need overall level script that has a variable tracking Thyra's last position?
            PlayerOn = false;

            player.transform.parent = null;

        }

    }
}

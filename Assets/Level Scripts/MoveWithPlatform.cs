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
            if (transform.position.y < other.transform.gameObject.transform.position.y)
            {
                PlayerOn = true;

                player.transform.parent = this.transform;
            }

            player.GetComponent<Rigidbody>().isKinematic = true;

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOn = false;

            player.transform.parent = null;
        }

    }

   
}

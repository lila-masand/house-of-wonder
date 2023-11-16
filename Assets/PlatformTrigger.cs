using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public Animation anim;
    //public GameObject obj;

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

        if(other.gameObject.name == "thyra" || other.gameObject.tag == "Player")
        {

            anim.Play("ActivatePlatform");

        }

    }

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.name == "thyra" || other.gameObject.tag == "Player")
        {

            anim.Play("ActivatePlatform");

        }

    }


}

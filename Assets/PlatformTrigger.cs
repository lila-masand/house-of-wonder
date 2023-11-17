using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public Animation anim;
    public Camera maincam;
    public Camera platformcam;
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
            platformcam.enabled = true;
            maincam.enabled = false;

            //if (Input.anyKey)
            //{
            //    maincam.enabled = true;
            //    platformcam.enabled = false;

            //}
            //Time.timeScale = .5f;

            //float pauseEnd = Time.realtimeSinceStartup + 5f;

            //while (Time.realtimeSinceStartup < pauseEnd)
            //{


            //}

            //Time.timeScale = 1f;
            //maincam.enabled = true;
            //platformcam.enabled = false;
        }

    }

    // need to have this method call a coroutine
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.name == "thyra" || other.gameObject.tag == "Player")
        {

            anim.Play("ActivatePlatform");
            platformcam.enabled = true;
            maincam.enabled = false;

            //if (Input.anyKey)
            //{
            //    maincam.enabled = true;
            //    platformcam.enabled = false;

            //}
            //Time.timeScale = 0f;

            //float pauseEnd = Time.realtimeSinceStartup + 5f;

            //while(Time.realtimeSinceStartup < pauseEnd)


            //Time.timeScale = 1f;
            //maincam.enabled = true;
            //platformcam.enabled = false;

        }

    }


}

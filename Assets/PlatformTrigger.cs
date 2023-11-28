using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public Animation anim;
    public Camera maincam;
    public Camera platformcam;

    bool activated = false;
    bool inRange = false;
    //public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        platformcam.enabled = false;
        maincam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && Input.GetKey(KeyCode.Return) && !activated)
        {
            StartCoroutine(WatchTrigger());
            activated = true;
        }


    }

    void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.name == "thyra" || other.gameObject.tag == "Player" && !activated)
        {
            inRange = true;
            //if (Input.GetKey(KeyCode.Return))
            //{
            //    StartCoroutine(WatchTrigger());
            //}

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

    void OnTriggerExit(Collider other)
    {

        inRange = false;


    }

    // need to have this method call a coroutine
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.name == "thyra" || other.gameObject.tag == "Player")
        {

            StartCoroutine(WatchTrigger());

           // platformcam.enabled = false;
           // maincam.enabled = true;

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

    IEnumerator WatchTrigger()
    {
        platformcam.enabled = true;
        maincam.enabled = false;

        anim.Play("ActivatePlatform");
        yield return new WaitForSeconds(1.5f);

        platformcam.enabled = false;
        maincam.enabled = true;
    }


}

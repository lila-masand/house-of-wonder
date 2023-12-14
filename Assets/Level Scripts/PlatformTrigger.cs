using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class PlatformTrigger : MonoBehaviour
{
    public Animation anim;
    public Camera maincam;
    public Camera platformcam;
    public TMP_Text ControlPopUp;
    public GameObject objToTrigger;

    public AudioClip activate_sound;

    bool activated = false;
    bool inRange = false;
    private bool? vertical;
    //public GameObject obj;

    void Start()
    {
        platformcam.enabled = false;
        ControlPopUp.enabled = false;
        vertical = null;

        if(objToTrigger != null)
        {
            if (objToTrigger.GetComponent<MovingPlatform>() != null)
                vertical = false;

            else if (objToTrigger.GetComponent<MovingPlatformVertical>() != null)
                vertical = true;
        }
    }

    void Update()
    {
        if (objToTrigger != null && vertical != null && !activated)
        {
            if (!vertical ?? false)
            {
                objToTrigger.GetComponent<MovingPlatform>().activated = false;
                vertical = null;
            }

            else if (vertical ?? false)
            {
                objToTrigger.GetComponent<MovingPlatformVertical>().activated = false;
                vertical = null;
            }
        }

        if (inRange && Input.GetKey(KeyCode.Return) && !activated)
        {     
            StartCoroutine(WatchTrigger());

            // Owen Ludlam
            AudioManager.instance.PlayEffect(gameObject, activate_sound, 0.5f);

            activated = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !activated)
        {
            inRange = true;
            ControlPopUp.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        inRange = false;
        ControlPopUp.enabled = false;
    }

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        StartCoroutine(WatchTrigger());         
    //    }
    //}

    IEnumerator WatchTrigger()
    {
        ControlPopUp.enabled = false;

        platformcam.enabled = true;

        anim.Play();
        yield return new WaitForSeconds(1.5f);

        platformcam.enabled = false;
        if (objToTrigger != null)
        {
            if (objToTrigger.GetComponent<MovingPlatform>() != null)
                objToTrigger.GetComponent<MovingPlatform>().activated = true;

            else if (objToTrigger.GetComponent<MovingPlatformVertical>() != null)
                objToTrigger.GetComponent<MovingPlatformVertical>().activated = true;
        }
    }
}

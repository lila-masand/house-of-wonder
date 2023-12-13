using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonPrompt : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_Text buttonPrompt;
    public GameObject player;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        buttonPrompt.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {

        buttonPrompt.enabled = false;
    }
}

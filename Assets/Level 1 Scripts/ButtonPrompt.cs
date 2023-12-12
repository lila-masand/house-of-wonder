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
        if ((player.transform.position - transform.position).magnitude < 1f)
        {

            buttonPrompt.enabled = true;

        }

        else
        {
            buttonPrompt.enabled = false;
        }
    }
}

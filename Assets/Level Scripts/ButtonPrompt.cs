using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Script by Lila Masand
public class ButtonPrompt : MonoBehaviour
{
    // Can be used for any TMP_Text object, not just button prompts
    public TMP_Text buttonPrompt;
    public GameObject player;

    void Start()
    {
        buttonPrompt.enabled = false;
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

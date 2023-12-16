using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Script by Lila Masand
public class ActivateSymbol : MonoBehaviour
{

    public TMP_Text buttonPrompt;
    public GameObject player;
    // Could be used for any object that has an animation with parameter "visible"
    public GameObject symbol;

    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated && (player.transform.position - transform.position).magnitude < 1f)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                symbol.GetComponent<Animator>().SetBool("visible", true);
                buttonPrompt.enabled = false;
                activated = true;
            }
        }      
    }
}

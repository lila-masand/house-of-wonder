using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePuzzle : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((player.transform.position - transform.position).magnitude < 2f && Input.GetKey(KeyCode.Return))
        {



        }
        
    }
}

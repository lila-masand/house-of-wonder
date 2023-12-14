using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMovingPlatformV : MonoBehaviour
{
    public int velocity = 4;
    public bool moving = false;
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
        if (other.gameObject.tag == "Player")
        {

            if (!moving)
            {
                this.transform.gameObject.AddComponent<MovingPlatformVertical>();
                this.transform.gameObject.GetComponent<MovingPlatformVertical>().velocity = velocity;
                moving = true;
            }
        }

    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script by Lila Masand
public class MakeMovingPlatformV : MonoBehaviour
{
    public int velocity = 4;
    public bool moving = false;

    // Script is overly specific now, can be made more general and reusable
    // Goal is to make a platform a vertically moving platform once the player jumps on it
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

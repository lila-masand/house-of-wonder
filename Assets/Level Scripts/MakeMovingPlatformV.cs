using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMovingPlatformV : MonoBehaviour
{
    public int velocity = 4;
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
            // Fix additional-velocity bug
            MovingPlatformVertical old_vertical = this.transform.gameObject.GetComponent<MovingPlatformVertical>();
            if (old_vertical != null)
            {
                Destroy(old_vertical);
            }

            this.transform.gameObject.AddComponent<MovingPlatformVertical>();
            this.transform.gameObject.GetComponent<MovingPlatformVertical>().velocity = velocity;
        }

    }

    
}

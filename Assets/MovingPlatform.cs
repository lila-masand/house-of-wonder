using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //public Transform target;
    
    public float velocity = 130f;

    private bool away = true;


    // Start is called before the first frame update
    void Start()
    {
        //target.position = new Vector3(2163.5f, 6f, 1770.5f);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.z < 1950.5f)
        {

            away = false;

        }

        else if(transform.position.z > 3340.116f)
        {
            away = true;

        }

        float step = velocity*Time.deltaTime;

        if (away)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(1938.74f, -137.53f, 1770.5f), step);
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(1938.74f, -137.53f, 3408.116f), step);

        }
      

    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //could maybe say that if target isn't null, raycast to target instead
    public GameObject target;
    public float velocity = 130f;

    //for debugging
    //public float raytarget;

    private bool away = true;
    private Vector3 origin;
    private RaycastHit hit;
    


    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;

        // It will start by moving away from its origin 
        Physics.Raycast(origin, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity);
    }

    // Update is called once per frame
    void Update()
    {
        if (away && transform.position.z < hit.point.z + 1.5)
        {
   
            Physics.Raycast(transform.position, new Vector3(0f, 0f, 1f), out hit, Mathf.Infinity);

            away = false;
        }

        else if (!away && transform.position.z > hit.point.z - 1.5)
        {
            Physics.Raycast(transform.position, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity);

            away = true;
        }

        float step = velocity*Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, hit.point, step);
      

    }
}

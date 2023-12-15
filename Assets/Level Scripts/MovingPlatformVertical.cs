using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovingPlatformVertical : MonoBehaviour
{
    //could maybe say that if target isn't null, raycast to target instead
    public GameObject target;
    public float velocity = 5f;
    public bool PlayerOn;
    public bool activated;
    //for debugging
    //public float raytarget;

    private bool away = true;
    private Vector3 origin;
    public RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;

        // It will start by moving away from its origin 
        Physics.Raycast(origin, new Vector3(0f, -1f, 0f), out hit, Mathf.Infinity);
        PlayerOn = false;
        activated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (away && transform.position.y < hit.point.y + 1.5)
            {

                Physics.Raycast(transform.position, new Vector3(0f, 1f, 0f), out hit, Mathf.Infinity);

                away = false;
            }

            else if (!away && transform.position.y > hit.point.y - 1.5)
            {
                Physics.Raycast(transform.position, new Vector3(0f, -1f, 0f), out hit, Mathf.Infinity);

                away = true;
            }

            float step = velocity * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, hit.point, step);
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (away && transform.position.y >= other.transform.gameObject.transform.position.y)
            {
                Physics.Raycast(transform.position, new Vector3(0f, 1f, 0f), out hit, Mathf.Infinity);
                away = true;
                float step = velocity * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, hit.point, step);

            }
            PlayerOn = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // need overall level script that has a variable tracking Thyra's last position?

            PlayerOn = false;

        }

    }
}

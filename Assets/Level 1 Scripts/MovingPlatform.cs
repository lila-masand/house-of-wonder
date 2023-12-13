using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //could maybe say that if target isn't null, raycast to target instead
    public GameObject target;
    public float velocity = 5f;
    public bool activated;
    public bool PlayerOn;
    public bool moveOnX = false;

    // Platform will start by moving away from its origin 
    private bool away = true;
    private Vector3 origin;
    private RaycastHit hit;
    
    void Start()
    {
        origin = transform.position;

        // moves horizontally on the z-axis
        if(!moveOnX)
            Physics.Raycast(origin, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity);
        // moves horizontally on the x-axis
        else if(moveOnX)
            Physics.Raycast(origin, new Vector3(-1f, 0f, 0f), out hit, Mathf.Infinity);

        PlayerOn = false;
        activated = true;
    }

    void Update()
    {
        if (activated)
        {
            if (!moveOnX)
            {
                // when the platform gets close enough to its Raycast target, switch direction
                if (away && transform.position.z < hit.point.z + 2)
                {
                    Physics.Raycast(transform.position, new Vector3(0f, 0f, 1f), out hit, Mathf.Infinity);
                    away = false;
                }

                else if (!away && transform.position.z > hit.point.z - 2)
                {
                    Physics.Raycast(transform.position, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity);
                    away = true;
                }

                float step = velocity * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, hit.point, step);
            }

            else if (moveOnX)
            {
                if (away && transform.position.x < hit.point.x + 2)
                {
                    Physics.Raycast(transform.position, new Vector3(1f, 0f, 0f), out hit, Mathf.Infinity);
                    away = false;
                }

                else if (!away && transform.position.x > hit.point.x - 2)
                {
                    Physics.Raycast(transform.position, new Vector3(-1f, 0f, 0f), out hit, Mathf.Infinity);
                    away = true;
                }

                float step = velocity * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, hit.point, step);
            }
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOn = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOn = false;

        }

    }
}

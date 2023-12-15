using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
// Script by Lila Masand
public class MovingPlatform : MonoBehaviour
{
    // Could maybe say that if target isn't null, raycast to target instead
    // Having two targets set would be less expensive as well
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

        // Moves horizontally on the z-axis
        if(!moveOnX)
            Physics.Raycast(origin, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity);
        // Moves horizontally on the x-axis
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
                // When the platform gets close enough to its Raycast target, switch direction
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


}

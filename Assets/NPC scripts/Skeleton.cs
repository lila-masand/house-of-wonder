using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{

    NavMeshAgent agent;
    Vector3 player;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        // player = GameObject.FindGameObjectWithTag("Player").transform.position;
        // UnityEngine.AI.NavMesh.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        // if (agent != null) {
            // agent.SetDestination(player);
        // }
    }
}


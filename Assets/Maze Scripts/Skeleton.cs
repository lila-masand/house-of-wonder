using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{

    private NavMeshAgent agent;
    private Transform player;
    private float detectRadius = 7.5f;
    private float roamRadius = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDist = Vector3.Distance(transform.position, player.position);

        if (playerDist <= detectRadius) {
            Vector3 oppositePlayer = transform.position - player.position;
            Vector3 targetPosition = transform.position + oppositePlayer.normalized * roamRadius;
            agent.SetDestination(targetPosition);
        } else {
            if (!agent.pathPending && agent.remainingDistance < 0.5f) {
                Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
                randomDirection += transform.position;
                NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamRadius, 1);
                agent.SetDestination(hit.position);
            }
        }
    }
}


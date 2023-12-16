using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInteract : MonoBehaviour
{
    public Maze maze;
    // public GameObject player;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDist = Vector3.Distance(transform.position, player.position);
        if (playerDist <= 0.9) {
            Destroy(gameObject);
            maze.collected++;
        }
    }

}

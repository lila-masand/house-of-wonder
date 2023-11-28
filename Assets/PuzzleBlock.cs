using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;


public class PuzzleBlock : MonoBehaviour
{
    public GameObject puzzle;
    bool allowed;

    // Start is called before the first frame update
    void Start()
    {
        //puzzle = GameObject.Find("Memory Blocks");
        allowed = false;
    }

    // Update is called once per frame
    void Update()
    {

        allowed = puzzle.GetComponent<Puzzle>().solutionInput;

    }

    void OnMouseOver()
    {
        if (allowed)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            
        }


    }

    void OnMouseExit()
    {
        if (allowed)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);

        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;


public class PuzzleBlock : MonoBehaviour
{
    public GameObject puzzle;
    bool allowed;
    public Material red;
    public Material white;

    private Puzzle puzzleScript;

    // Start is called before the first frame update
    void Start()
    {
        //puzzle = GameObject.Find("Memory Blocks");
        allowed = false;
        puzzleScript = puzzle.GetComponent<Puzzle>();
    }

    // Update is called once per frame
    void Update()
    {

        allowed = puzzleScript.solutionInput;

    }

    void OnMouseOver()
    {
        if (allowed)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            //this.GetComponent<MeshRenderer>().material = red;


        }


    }

    void OnMouseExit()
    {
        if (allowed)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
            //this.GetComponent<MeshRenderer>().material = white;


        }


    }

    void OnMouseDown()
    {
        puzzleScript.userSolution.Add(transform.GetSiblingIndex());
    }
}

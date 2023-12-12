using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;


public class SymbolBlock : MonoBehaviour
{
    public GameObject puzzle;
    bool allowed;
    //public Material red;
    //public Material white;

    private SymbolPuzzle puzzleScript;

    // Start is called before the first frame update
    void Start()
    {
        //puzzle = GameObject.Find("Memory Blocks");
        //allowed = false;
        puzzleScript = puzzle.GetComponent<SymbolPuzzle>();
    }

    // Update is called once per frame
    void Update()
    {

        allowed = puzzleScript.solutionInput;

    }

    void OnMouseOver()
    {
        //if (allowed)
        //{
        //    this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            //this.GetComponent<Renderer>().material = red;


        //}


    }

    void OnMouseExit()
    {
        //if (allowed)
        //{
         //   this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
            //this.GetComponent<Renderer>().material = white;


        //}


    }

    void OnMouseDown()
    {
        puzzleScript.userSolution.Add(transform.GetSiblingIndex());
        //this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        GetComponent<Animator>().SetBool("Flash", true);
    }
}

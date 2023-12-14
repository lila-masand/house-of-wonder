using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;


public class SymbolBlock : MonoBehaviour
{
    public GameObject puzzle;
    bool allowed;

    private SymbolPuzzle puzzleScript;

    // Start is called before the first frame update
    void Start()
    {
        puzzleScript = puzzle.GetComponent<SymbolPuzzle>();
    }

    // Update is called once per frame
    void Update()
    {

        allowed = puzzleScript.solutionInput;

    }

    void OnMouseDown()
    {
        puzzleScript.userSolution.Add(transform.GetSiblingIndex());
        GetComponent<Animator>().SetBool("Flash", true);
    }
}

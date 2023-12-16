using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// Script by Lila Masand
public class PuzzleTile : MonoBehaviour
{
    // Used with FloorPuzzle for each individual tile
    public GameObject puzzle;
    public bool allowed;

    private FloorPuzzle puzzleScript;
    private bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        puzzle = ((transform.parent).transform.parent).gameObject;
        puzzleScript = puzzle.GetComponent<FloorPuzzle>();
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        allowed = puzzleScript.solutionInput;   
    }

    // When the player steps on a tile, check if it's the correct one based on the solution
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && allowed)
        {
            this.GetComponent<Animator>().SetBool("On", true);
            puzzleScript.userSolution.Add(transform.parent.GetSiblingIndex() + "" + transform.GetSiblingIndex());
            puzzleScript.userInputNum++;

            if (puzzleScript.solCheckable[puzzleScript.userInputNum - 1] != puzzleScript.userSolution[puzzleScript.userInputNum - 1])
            {
                puzzleScript.correct = false;
            }

            pressed = true;
        }
    }
}

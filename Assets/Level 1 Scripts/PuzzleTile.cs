using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
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

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && allowed)
        {
            this.GetComponent<Animator>().SetBool("On", true);
            puzzleScript.userSolution.Add(transform.parent.GetSiblingIndex() + "" + transform.GetSiblingIndex());
            puzzleScript.userInputNum++;

            if (puzzleScript.solCheckable[puzzleScript.userInputNum - 1] != puzzleScript.userSolution[puzzleScript.userInputNum - 1])
            {
                UnityEngine.Debug.Log(puzzleScript.solCheckable[puzzleScript.userInputNum - 1]);
                UnityEngine.Debug.Log(puzzleScript.userSolution[puzzleScript.userInputNum - 1]);

                puzzleScript.correct = false;

            }

            pressed = true;
        }
    }

}

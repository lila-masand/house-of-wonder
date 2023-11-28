using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Puzzle : MonoBehaviour
{

    public Camera PuzzleCam;
    public Camera MainCam;
    //public Animation anim;
    public GameObject player;

    bool puzzlePlay;
    public bool solutionInput;
    int[] solution;
    int blockNum;


    void Start()
    {
        solution = getPuzzle();
        blockNum = 0;
        puzzlePlay = false;
        solutionInput = false;

    }

    // Update is called once per frame
    void Update()
    {
        // automatically switch to puzzle camera when in range
        if(player.transform.position.x > 39f && player.transform.position.z > 373f && player.transform.position.z < 377f)
        {

            MainCam.enabled = false;
            PuzzleCam.enabled = true;

        }

        else
        {
            MainCam.enabled = true;
            PuzzleCam.enabled = false;
        }

  
        if(PuzzleCam.enabled && Input.GetKey(KeyCode.Return))
        {

            puzzlePlay = true;

            StartCoroutine(RunPuzzle());

            puzzlePlay = false;
            solutionInput = true;
        }


        else if(PuzzleCam.enabled && solutionInput)
        {



        }

    }

    IEnumerator RunPuzzle()
    {
        
        for (int i = 0; i < 4; i++)
        {

            transform.GetChild(solution[i]).GetComponent<Animator>().SetBool("Flash", true);
            yield return new WaitForSeconds(1.5f);
            transform.GetChild(solution[i]).GetComponent<Animator>().SetBool("Flash", false);
        }

    }
  

    private int[] getPuzzle()
    {
        System.Random picker = new System.Random();

        // pick 4 random blocks
        return new int[] { picker.Next(9), picker.Next(9), picker.Next(9), picker.Next(9)};

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;


public class Puzzle : MonoBehaviour
{

    public Camera PuzzleCam;
    public Camera MainCam;
    public Camera PlayerCam;
    public GameObject player;
    public GameObject LoadZone;
    public List<int> userSolution;
    public bool solutionInput;
    public bool solved;
    public TMP_Text ControlPopUp;



    private int[] solution;
    private GameObject parent;
    //private CinemachineBrain cameraBrain;

    void Start()
    {
        solution = getPuzzle();
        solutionInput = false;
        userSolution = new List<int>();
        parent = transform.parent.gameObject;
        solved = false;
        MainCam.enabled = true;
        ControlPopUp.enabled = false;
        //cameraBrain = MainCam.GetComponent<CinemachineBrain>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!solved)
        {
            // automatically switch to puzzle camera when in range
            if (player.transform.position.x > 32f && player.transform.position.z > 371.5f && player.transform.position.z <= 373f)
            {
                //MainCam.enabled = false;
                PuzzleCam.enabled = true;
            }

            else
            {

                //MainCam.enabled = true;
                PuzzleCam.enabled = false;
            }


            if (!solutionInput && PuzzleCam.enabled && Input.GetKey(KeyCode.Return))
            {
                StartCoroutine(RunPuzzle());

                solutionInput = true;
            }


            else if (solutionInput && PuzzleCam.enabled && userSolution.Count == 4)
            {
                bool correct = true;

                for (int i = 0; i < 4; i++)
                {
                    if (solution[i] != userSolution[i])
                    {
                        correct = false;
                        break;
                    }

                }

                if (correct)
                {

                    PuzzleCam.enabled = false;
                    solved = true;
                    StartCoroutine(DoorActivate());

                }

                else
                {

                    solutionInput = false;
                    solution = getPuzzle();
                    userSolution.Clear();

                    StartCoroutine(FlashAll());


                }

            }
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

    IEnumerator FlashAll()
    {
        for (int i = 0; i < 9; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("Flash", true);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 9; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("Flash", false);
        }

    }

    IEnumerator DoorActivate()
    {
        LoadZone.GetComponent<Animator>().SetBool("PuzzleSolved", true);
        PlayerCam.enabled = false;
        yield return new WaitForSeconds(2f);
        PlayerCam.enabled = true;
        MainCam.enabled = false;
        //MainCam.targetDisplay = 2;
    }


    private int[] getPuzzle()
    {
        System.Random picker = new System.Random();

        // pick 4 random blocks
        return new int[] { picker.Next(9), picker.Next(9), picker.Next(9), picker.Next(9)};

    }
}

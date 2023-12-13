using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using System.Security.Cryptography;

public class SymbolPuzzle : MonoBehaviour
{

    public Camera PuzzleCam;
    //public Camera MainCam;
    public Camera PlayerCam;
    public GameObject player;
    //public GameObject LoadZone;
    public List<int> userSolution;
    public bool solutionInput;
    public bool solved;
    public TMP_Text ControlPopUp;
    public GameObject objToTrigger;



    private int[] solution;
    private Transform tileMM;
    //private CinemachineBrain cameraBrain;

    void Start()
    {
        solution = getPuzzle();
        solutionInput = false;
        userSolution = new List<int>();
        solved = false;
        //MainCam.enabled = true;
        //ControlPopUp.enabled = false;
        //cameraBrain = MainCam.GetComponent<CinemachineBrain>();
        tileMM = transform.GetChild(4);


    }

    // Update is called once per frame
    void Update()
    {
        if (!solved)
        {
            // bring up the control prompt when in range
            if ((player.transform.position - tileMM.transform.position).magnitude < 1.5f && !PuzzleCam.enabled)
            {
                //MainCam.enabled = false;
                //ControlPopUp.enabled = true;

                if (Input.GetKey(KeyCode.Return))
                {
                    PuzzleCam.enabled = true;
                    ControlPopUp.enabled = false;
                    solutionInput = true;
                }
            }
            else if((player.transform.position - tileMM.transform.position).magnitude > 1.5f)
            {
                //MainCam.enabled = true;
                PuzzleCam.enabled = false;
                //ControlPopUp.enabled = false;
            }

            if (solutionInput && PuzzleCam.enabled && userSolution.Count == 4)
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
                    //StartCoroutine(DoorActivate());
                }

                else
                {

                    userSolution.Clear();
                    StartCoroutine(FlashAll());
                }

            }
        }
    }

    IEnumerator FlashAll()
    {
        yield return new WaitForSeconds(1f);

        

        //GetComponent<Animator>().SetTrigger("Wrong");

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


    IEnumerator ObjActivate()
    {
        //LoadZone.GetComponent<Animator>().SetBool("PuzzleSolved", true);

        PuzzleCam.enabled = false;
        PlayerCam.enabled = false;
        //MainCam.enabled = true;
        yield return new WaitForSeconds(0.5f);
        objToTrigger.GetComponent<Animator>().SetBool("activated", true);
        yield return new WaitForSeconds(3f);
        PlayerCam.enabled = true;
        //MainCam.enabled = false;
        ControlPopUp.enabled = false;
        //MainCam.targetDisplay = 2;
    }

    IEnumerator DoorActivate()
    {
        ControlPopUp.enabled = false;

        //LoadZone.GetComponent<Animator>().SetBool("PuzzleSolved", true);
        PlayerCam.enabled = false;
        yield return new WaitForSeconds(2f);
        PlayerCam.enabled = true;
        //MainCam.enabled = false;
        //MainCam.targetDisplay = 2;
    }


    private int[] getPuzzle()
    {
        System.Random picker = new System.Random();

        // pick 4 random blocks
        return new int[] { picker.Next(9), picker.Next(9), picker.Next(9), picker.Next(9) };
    }
}

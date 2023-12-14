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
    public int puzzleLength = 4;


    public List<int> solution;
    private Transform tileMM;
    //public CinemachineVirtualCamera vcam;
    public CinemachineStateDrivenCamera statecam;


    //private CinemachineBrain cameraBrain;

    void Start()
    {
        // solution = getPuzzle();
        Debug.Log(solution);
        solutionInput = false;
        userSolution = new List<int>();
        solved = false;
        //MainCam.enabled = true;
        //ControlPopUp.enabled = false;
        //cameraBrain = MainCam.GetComponent<CinemachineBrain>();
        ControlPopUp.enabled = false;

        tileMM = transform.GetChild(4);
        if(statecam != null)
            statecam.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (!solved)
        {
            // bring up the control prompt when in range
            if ((player.transform.position - tileMM.transform.position).magnitude < 3.0f && !PuzzleCam.enabled)
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
            else if((player.transform.position - tileMM.transform.position).magnitude > 3.0f)
            {
                //MainCam.enabled = true;
                PuzzleCam.enabled = false;
                //ControlPopUp.enabled = false;
            }

            if (solutionInput && PuzzleCam.enabled && userSolution.Count == puzzleLength)
            {
                // check if solution[i] is in userSolution, since it does not have to be in any specific order: like python's correct = True if x in array else False
                bool correct = false;

                for (int i = 0; i < puzzleLength; i++)
                {
                    correct = false;
                    for (int j = 0; j < puzzleLength; j++) {
                        if (solution[i] != userSolution[j]) { 
                            correct = true;
                            break;
                        }
                    }
                    if (!correct) { break; }

                }

                if (correct)
                {
                    //vcam.m_Priority = 10;
                    // statecam.enabled = true; // not set
                    PuzzleCam.enabled = false;
                    solved = true;
                    Debug.Log("Puzzle solved");
                    // StartCoroutine(ObjActivate());
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
        yield break;
        //LoadZone.GetComponent<Animator>().SetBool("PuzzleSolved", true);
        //statecam.enabled = true;
        //MainCam.enabled = true;

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
        //vcam.m_Priority = 9;
        //statecam.m_Priority = 9;
        statecam.enabled = false;

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

    private void Shuffle<T>(ref List<T> list)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public List<int> getPuzzle()
    {
        //System.Random picker = new System.Random();

        // pick 4 random blocks
        //return new int[] { picker.Next(9), picker.Next(9), picker.Next(9)};
        if (puzzleLength == 3)
        {
            return new List<int> { 3, 4, 8 };
        }

        else if(puzzleLength == 4)
        {
            List<int> possibilities = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            Shuffle<int>(ref possibilities);

            return new List<int> { possibilities[0], possibilities[1], possibilities[2], possibilities[3]};
        }

        return new List<int> { 0 };

    }
}

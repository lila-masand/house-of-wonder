using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using System.Security.Cryptography;
using System.Linq;

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
    private GameObject middleWall;
    private GameObject puzzleObj;


    //private CinemachineBrain cameraBrain;

    void Start()
    {
        // solution = getPuzzle();
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

        puzzleObj = GameObject.Find("Puzzle");
        middleWall = GameObject.Find("MIDDLE");
        // Debug.Log(middleWall);
        // Debug.Log(puzzleObj);

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
            
            if (solutionInput && PuzzleCam.enabled && userSolution.Distinct().Count() == puzzleLength)
            {
                bool correct = solution.ToHashSet().SetEquals(userSolution.ToHashSet());
                if (correct)
                {
                    //vcam.m_Priority = 10;
                    // statecam.enabled = true; // not set
                    PuzzleCam.enabled = false;
                    solved = true;
                    Debug.Log("Puzzle solved");
                    if (solved) {
                        MoveDown(gameObject);
                        // MoveDown(puzzleObj);
                        // MoveDown(middleWall);
                        MoveDown(GameObject.Find("MIDDLE"));
                        MoveDown(GameObject.Find("Puzzle"));
                    }
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

    void MoveDown(GameObject obj) {
        if (obj != null) {
            // Debug.Log("Move down");
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - Time.deltaTime * 2.0f, obj.transform.position.z);

            // check if the puzzle is below or at specific spot
            // if (GameObject.Find("Puzzle").transform.position.y <= -5.9f) {
            //     solved = false;
            // }
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

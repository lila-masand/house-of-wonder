using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using Cinemachine;
// Script by Lila Masand
//  Last updated 2023

public class FloorPuzzle : MonoBehaviour
{

    public Camera PuzzleCam;
    public Camera MainCam;
    public Camera PlayerCam;
    public GameObject player;
    public GameObject puzzleSwitch;


    public GameObject LoadZone;
    public GameObject toTrigger;
    //public GameObject platform;


    public List<string> userSolution;
    public List<string> solCheckable;
    public int userInputNum;
    public bool solutionInput;
    public bool solved;
    public bool correct;
    public bool isPlaying;
    public Animation anim;
    public TMP_Text buttonPrompt;

    // one list for each column
    private List<int>[] solution;
    private Transform tile00;
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;
    public CinemachineStateDrivenCamera statecam;

    //private GameObject parent;
    //private CinemachineBrain cameraBrain;

    void Start()
    {
        solution = getPuzzle();

        solutionInput = false;
        userSolution = new List<string>();
        //parent = transform.parent.gameObject;
        solved = false;
        MainCam.enabled = true;
        tile00 = (transform.GetChild(0)).transform.GetChild(0);
        correct = true;
        userInputNum = 0;
        userSolution = new List<string>();
        solCheckable = new List<string>();
        PuzzleCam.enabled = false;
        isPlaying = false;
        //solution = getPuzzle();
        statecam.enabled = false;

        //ControlPopUp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!solved)
        {
            // automatically switch to puzzle camera when switch pressed
            //if ((player.transform.position - tile00.transform.position).magnitude < 1f)
            //{
            //    PuzzleCam.enabled = true;
            //}

            //else
            //{
            //    PuzzleCam.enabled = false;
            //}


            if (!isPlaying && !solutionInput && (player.transform.position - puzzleSwitch.transform.position).magnitude < 3f)
            {
                //solution = getPuzzle();
                //PuzzleCam.enabled = true;
                //buttonPrompt.enabled = true;

                if (Input.GetKey(KeyCode.Return))
                {

                    puzzleSwitch.GetComponent<MeshRenderer>().material.color = new Color(255f, 1f, 1f, .5f);
                    buttonPrompt.enabled = false;


                    StartCoroutine(RunPuzzle());


                    if (solCheckable.Count < 5)
                    {
                        solCheckable = AddSolution();

                    }

                }
            }

            else if (isPlaying)
            {

                buttonPrompt.enabled = false;
            }

            if (solutionInput)
            {
                //UnityEngine.Debug.Log(solCheckable.Count);
                //buttonPrompt.enabled = false;

                if (!correct)
                {
                    StartCoroutine(FlashAll());
                    puzzleSwitch.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, .5f);

                    solutionInput = false;
                    solution = getPuzzle();
                    userSolution.Clear();
                    solCheckable.Clear();
                    userInputNum = 0;

                    
                    correct = true;
                    solved = false;


                }

                else if((userSolution.Count == solCheckable.Count) && correct)
                {
                    //UnityEngine.Debug.Log(solCheckable.Count);

                    //UnityEngine.Debug.Log(userSolution.Count);
                    //vcam1.m_Priority = 10;
                    //vcam2.m_Priority = 10;
                    //statecam.m_Priority = 10;
                    statecam.enabled = true;
                    //statecam.SetActive(true);

                    StartCoroutine(ObjActivate());
                    puzzleSwitch.GetComponent<MeshRenderer>().material.color = new Color(1f, 255f, 1f, .5f);
                   

                    solved = true;
                    solutionInput = false;
                }

               
                //coroutine for inputing the player's choices as they walk over the tiles?
                

            }
        }
    }


    IEnumerator RunPuzzle()
    {
        PuzzleCam.enabled = true;
        PlayerCam.enabled = false;
        isPlaying = true;


        for (int i = 0; i < 6; i++)
        {
            //could also make an array of the GameObjects called grid
            Transform currColumn = transform.GetChild(i);

            for (int j = 0; j < solution[i].Count; j++) {

                currColumn.GetChild(solution[i][j]).GetComponent<Animator>().SetBool("On", true);
                yield return new WaitForSeconds(1f);
            }

        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 6; i++)
        {
            Transform currColumn = transform.GetChild(i);

            for (int j = 0; j < solution[i].Count; j++)
            {
                currColumn.GetChild(solution[i][j]).GetComponent<Animator>().SetBool("On", false);
            }
        }

        solutionInput = true;
        PuzzleCam.enabled = false;
        PlayerCam.enabled = true;
        isPlaying = false;

    }

    IEnumerator CheckTiles()
    {
        yield return null;

    }

    private List<string> AddSolution()
    {
        List<string> sol = new List<string>();

        for (int i = 0; i < 6; i++)
        {
            //could also make an array of the GameObjects called grid
            Transform currColumn = transform.GetChild(i);

            for (int j = 0; j < solution[i].Count; j++)
            {

                sol.Add(i + "" + solution[i][j]);
                //UnityEngine.Debug.Log(i + "" + solution[i][j] + ", ");
            }

        }

        return sol;
    }

    IEnumerator FlashAll()
    {

        for (int i = 0; i < 6; i++)
        {
            Transform currColumn = transform.GetChild(i);

            for (int j = 0; j < 5; j++)
            {
                currColumn.GetChild(j).GetComponent<Animator>().SetBool("On", true);
            }
        }
       
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 6; i++)
        {
            Transform currColumn = transform.GetChild(i);

            for (int j = 0; j < 5; j++)
            {
                currColumn.GetChild(j).GetComponent<Animator>().SetBool("On", false);
            }
        }

    }

    IEnumerator ObjActivate()
    {
        //LoadZone.GetComponent<Animator>().SetBool("PuzzleSolved", true);
        
        PuzzleCam.enabled = false;
        PlayerCam.enabled = false;
        MainCam.enabled = true;
        yield return new WaitForSeconds(0.5f);
        toTrigger.GetComponent<Animator>().SetBool("activated", true);
        yield return new WaitForSeconds(3f);
        PlayerCam.enabled = true;
        MainCam.enabled = false;
        buttonPrompt.enabled = false;

        //vcam1.m_Priority = 9;
        //vcam2.m_Priority = 9;
        //statecam.m_Priority = 9;

        statecam.enabled = false;

        //MainCam.targetDisplay = 2;
    }


    public List<int>[] getPuzzle()
    {
        System.Random picker = new System.Random();
        List<int>[] newSolution = { new List<int> (), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>() };

        //for(int i = 0;i < 9; i++)
        //{
        //    newSolution[i] = new List<int> ();
        //}

        int currRow = picker.Next(4);

        newSolution[0].Add(currRow);
        int tileCount = 1;

        // 0 = up, 1 = right
        for (int i = 0; i < 5; i++)
        {
            int nextTile = -1;
            bool done = false;
            while (!done && tileCount <= 9)
            {
                //UnityEngine.Debug.Log(i);
                //nextTile = -1;

                if (tileCount > 1 && newSolution[i].Count < 3 && currRow < 4)
                {
                    nextTile = picker.Next(2);
                }

                // if there have already been 3 tiles chosen in this column, automatically go right
                // same if we reached the top of the puzzle
                else
                {
                    nextTile = 1;
                    done = true;
                }

                switch (nextTile)
                {
                    case 0:
                        {
                            currRow++;
                            newSolution[i].Add(currRow);
                            //UnityEngine.Debug.Log(i + "" + currRow + "\n");
                            //solCheckable.Add(i + "" + currRow);
                            break;
                        }

                    case 1:
                        {
                            newSolution[i + 1].Add(currRow);
                            //UnityEngine.Debug.Log((i + 1) + "" + currRow + "\n");
                            //solCheckable.Add(i + "" + currRow);

                            done = true;
                            //i++;
                            break;
                        }
                }

                //solCheckable.Add(i + "" + currRow);


                tileCount++;
            }

        }

        //UnityEngine.Debug.Log(solCheckable.Count);
        return newSolution;

    }
}

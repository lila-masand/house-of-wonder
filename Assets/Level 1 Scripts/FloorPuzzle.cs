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
    public CinemachineStateDrivenCamera statecam;

    //private CinemachineBrain cameraBrain;

    void Start()
    {
        solution = getPuzzle();

        solutionInput = false;
        userSolution = new List<string>();
        solved = false;
        MainCam.enabled = true;
        tile00 = (transform.GetChild(0)).transform.GetChild(0);
        correct = true;
        userInputNum = 0;
        userSolution = new List<string>();
        solCheckable = new List<string>();
        PuzzleCam.enabled = false;
        isPlaying = false;
        statecam.enabled = false;

        //ControlPopUp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!solved)
        {
            if (!isPlaying && !solutionInput && (player.transform.position - puzzleSwitch.transform.position).magnitude < 3f && Input.GetKey(KeyCode.Return))
            {
                puzzleSwitch.GetComponent<MeshRenderer>().material.color = new Color(255f, 1f, 1f, .5f);
                buttonPrompt.enabled = false;

                StartCoroutine(RunPuzzle());

                if (solCheckable.Count < 5)
                    solCheckable = AddSolution();                
            }

            else if (isPlaying)
            {
                buttonPrompt.enabled = false;
            }

            if (solutionInput)
            {
                //UnityEngine.Debug.Log(solCheckable.Count);
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

                    statecam.enabled = true;

                    StartCoroutine(ObjActivate());
                    puzzleSwitch.GetComponent<MeshRenderer>().material.color = new Color(1f, 255f, 1f, .5f);
                   
                    solved = true;
                    solutionInput = false;
                }

            }
        }
    }


    IEnumerator RunPuzzle()
    {
        PuzzleCam.enabled = true;
        PlayerCam.enabled = false;
        isPlaying = true;

        // turn on tiles one at a time
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

        // turn them all off at once
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

    private List<string> AddSolution()
    {
        List<string> sol = new List<string>();

        for (int i = 0; i < 6; i++)
        {
            Transform currColumn = transform.GetChild(i);

            for (int j = 0; j < solution[i].Count; j++)
            {

                sol.Add(i + "" + solution[i][j]);
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
        PuzzleCam.enabled = false;
        PlayerCam.enabled = false;
        yield return new WaitForSeconds(0.5f);
        toTrigger.GetComponent<Animator>().SetBool("activated", true);
        yield return new WaitForSeconds(3f);
        PlayerCam.enabled = true;
        buttonPrompt.enabled = false;
        statecam.enabled = false;
    }


    public List<int>[] getPuzzle()
    {
        System.Random picker = new System.Random();
        List<int>[] newSolution = { new List<int> (), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>() };

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
                            break;
                        }

                    case 1:
                        {
                            newSolution[i + 1].Add(currRow);
                            done = true;
                            break;
                        }
                }
                tileCount++;
            }

        }
        return newSolution;
    }
}

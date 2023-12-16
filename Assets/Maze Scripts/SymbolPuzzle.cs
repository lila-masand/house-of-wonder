using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using System.Security.Cryptography;
using System.Linq;

// Script by Lila Masand
public class SymbolPuzzle : MonoBehaviour
{

    public Camera PuzzleCam;
    public Camera MainCam;
    public Camera PlayerCam;
    public GameObject player;
    public List<int> userSolution;
    public bool solutionInput;
    public bool solved;
    public TMP_Text ControlPopUp;
    public GameObject objToTrigger;
    public int puzzleLength = 4;
    public List<int> solution;
    public CinemachineStateDrivenCamera statecam;
    // SFX - Owen Ludlam
    public AudioClip activate_obj_sfx;
    public bool multiCam = false;

    private Transform tileMM;

    void Awake()
    {
        // Get solution early so other scripts can access it
        solution = getPuzzle();
    }

    void Start()
    {
        solutionInput = false;
        userSolution = new List<int>();
        solved = false;
        ControlPopUp.enabled = false;

        // Get middle tile to use as a reference for distance from the puzzle
        tileMM = transform.GetChild(4);

        // Check if there's a state-driven Cinemachine camera
        if (statecam != null)
        {
            statecam.enabled = false;
        }

        MainCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!solved)
        {
            // Allow player to start puzzle when in range
            if ((player.transform.position - tileMM.transform.position).magnitude < 1.5f && !PuzzleCam.enabled)
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    PuzzleCam.enabled = true;
                    ControlPopUp.enabled = false;
                    solutionInput = true;
                }
            }
            // Break out of puzzle camera when the player moves away
            else if((player.transform.position - tileMM.transform.position).magnitude > 1.5f)
            {
                PuzzleCam.enabled = false;
            }
            // Check whether the user is done putting in their solution
            if (solutionInput && PuzzleCam.enabled && userSolution.Distinct().Count() == puzzleLength)
            {
                bool correct = solution.ToHashSet().SetEquals(userSolution.ToHashSet()); // (A)
                
                if (correct)
                {
                    AudioManager.instance.PlayEffect(gameObject, AudioManager.DefaultClips.SUCCESS); // Owen
                    
                    statecam.enabled = true;
                    MainCam.enabled = true;

                    PuzzleCam.enabled = false;
                    solved = true;
                    Debug.Log("Puzzle solved");
                    if (solved) {

                        if (activate_obj_sfx != null)
                        {
                            AudioManager.instance.PlayEffect(gameObject, activate_obj_sfx);
                        }
                    }
                    StartCoroutine(ObjActivate());
                }

                else
                {
                    // If the puzzle is failed, flash all tiles and reset
                    AudioManager.instance.PlayEffect(gameObject, AudioManager.DefaultClips.FAIL); // Owen
                    userSolution.Clear();
                    StartCoroutine(FlashAll());
                }

            }
        }
    }


    IEnumerator FlashAll()
    {
        yield return new WaitForSeconds(1f);

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

    // Any object with an Animator and a parameter "activated" can be triggered by solving the puzzle
    IEnumerator ObjActivate()
    {
        PuzzleCam.enabled = false;
        PlayerCam.enabled = false;
        yield return new WaitForSeconds(0.5f);
        objToTrigger.GetComponent<Animator>().SetBool("activated", true);
        yield return new WaitForSeconds(3f);
        PlayerCam.enabled = true;
        ControlPopUp.enabled = false;
        statecam.enabled = false;
        MainCam.enabled = false;
    }

    // (A)
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
            List<int> possibilities = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; // (A)
            Shuffle<int>(ref possibilities); // (A)

            return new List<int> { possibilities[0], possibilities[1], possibilities[2], possibilities[3]}; // (A)
        }

        return new List<int> { 0 };
    }
}

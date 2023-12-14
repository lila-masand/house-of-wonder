using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public Canvas canvas;
    
    private bool is_paused = false;

    public static PauseMenuController instance;

    private void Awake()
    {
        // Only one pause menu
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Persist the pause menu between scenes
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(canvas);

        // Do not pause until start is clicked
        gameObject.SetActive(false);
    }

    private void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!is_paused)
            {
                is_paused = true;
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                is_paused = false;
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}

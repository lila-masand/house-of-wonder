using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Owen Ludlam
public class PauseMenuController : MonoBehaviour
{
    public Canvas canvas;
    public Button continue_button;
    public Slider volume_slider;

    private bool is_paused = false;

    public static PauseMenuController instance;

    private void Awake()
    {
        // Only one pause menu
        if(instance == null)
        {
            instance = this;
            // Persist the pause menu between scenes
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(this);
        }

        // Do not pause until start is clicked
        gameObject.SetActive(false);
    }

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        continue_button.onClick.AddListener(UnPause);
        volume_slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(volume_slider.value); });
    }

    // Unpause the game
    private void UnPause()
    {
        is_paused = false;
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    // Pause the game
    private void Pause()
    {
        is_paused = true;
        volume_slider.value = AudioManager.instance.global_volume;
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!is_paused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }

    }
}

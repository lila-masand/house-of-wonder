using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Scripts by Owen Ludlam
public class MenuController : MonoBehaviour
{
    public AudioClip level_1_track;

    // Start Game button reference
    public Button start_button;

    // String name of the starting level
    public string start_scene_name;

    void Start()
    {
        start_button.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        AudioManager.instance.SwapTracks(level_1_track);
        SceneManager.LoadScene(start_scene_name);
    }
}

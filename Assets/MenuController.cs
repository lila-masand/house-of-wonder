using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Scripts by Owen Ludlam
public class MenuController : MonoBehaviour
{
    public AudioClip level_1_track;
    public AudioClip click_sound;
    public AudioSource menu_source;

    // Start Game button reference
    public Button start_button;
    public string level_1_scene;

    // String name of the starting level
    // public string ;

    void Start()
    {
        menu_source.clip = click_sound;
        start_button.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        StartCoroutine(FinishClick());
    }

    private IEnumerator FinishClick()
    {
        menu_source.Play();
        
        while (menu_source.isPlaying)
        {
            yield return null;
        }

        AudioManager.instance.SwapTracks(level_1_track);
        SceneManager.LoadScene(level_1_scene, LoadSceneMode.Single); // Load single to fix lighting issues
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.SwapTracks(AudioManager.MusicTracks.MOON); // Owen Ludlam
        SceneManager.LoadScene("Maze Level");

        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Maze Level"));     
    }
}

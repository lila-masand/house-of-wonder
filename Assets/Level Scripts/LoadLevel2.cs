using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// Script by Lila Masand
public class LoadLevel2 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.SwapTracks(AudioManager.MusicTracks.SPOOK); // Owen Ludlam
        SceneManager.LoadScene("Level 2");
    }
}

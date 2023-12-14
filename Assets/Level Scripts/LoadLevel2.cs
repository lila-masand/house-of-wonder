using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel2 : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.SwapTracks(AudioManager.MusicTracks.SPOOK); // Owen Ludlam
        SceneManager.LoadScene("Level 2");
    }


    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(0.1f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 2"));
    }
}

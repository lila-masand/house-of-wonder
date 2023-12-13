using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Starting audio
    public AudioClip initial_track;

    // Sources to play audio from
    private AudioSource track_0;
    private AudioSource track_1;
    private bool current_track;

    // AudioManager instance for reference
    public static AudioManager instance;

    private void Awake()
    {
        // Guarentee only 1 instance at a time
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance); // Allow audiosource to pass between scenes
        }
        else
        {
            Destroy(this); // Destroy duplicate AudioManagers
        }

    }

    private void Start()
    {
        // Create audio sources to play clips from
        track_0 = gameObject.AddComponent<AudioSource>();
        track_1 = gameObject.AddComponent<AudioSource>();

        // Loop music
        track_0.loop = true;
        track_1.loop = true;

        // Play track_0 on start
        current_track = true;
        track_0.clip = initial_track;
        track_0.Play();
    }

    // Swap between audio sources with a fade effect
    public void SwapTracks(AudioClip clip, float fadeTime = 1f)
    {
        // Stop other coroutines to prevent sounds from getting entertwined
        StopAllCoroutines();
        
        // Swap the current music track
        if (current_track)
        {
            track_1.clip = clip;
            track_1.Play();
            current_track = false;

            // Use a coroutine to fade between tracks
            StartCoroutine(Fade(track_1, track_0, fadeTime));
        }
        else
        {
            track_0.clip = clip;
            track_0.Play();
            current_track = true;

            // Use a coroutine to fade between tracks
            StartCoroutine(Fade(track_0, track_1, fadeTime));
        }
    }

    // Fade between music tracks
    private IEnumerator Fade(AudioSource source_1, AudioSource source_2, float fadeTime)
    {
        float elapsed = 0;
        while (elapsed < fadeTime)
        {
            // Use linear interpolation to create the fade effect
            source_1.volume = Mathf.Lerp(0, 1, elapsed / fadeTime);
            source_2.volume = Mathf.Lerp(1, 0, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Stop playing music when the volume is 0
        source_2.Stop();
    }
}

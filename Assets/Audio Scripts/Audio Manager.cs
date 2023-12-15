using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

// Owen Ludlam
public class AudioManager : MonoBehaviour
{
public enum DefaultClips
{
    SUCCESS,
    FAIL,
    ACTIVATE,
    INTERACT
}

public enum MusicTracks
{
    MOON,
    SPACE,
    POLY,
    SPOOK
}

    // Starting audio
    public AudioClip initial_track;

    // Sources to play audio from
    private AudioSource track_0;
    private AudioSource track_1;
    private bool current_track;

    public float global_volume = 1f;

    // Default Audio Clips
    public AudioClip success_sfx;
    public AudioClip fail_sfx;
    public AudioClip interact_sfx;
    public AudioClip activate;

    // Default Music Tracks
    public AudioClip moon_track;
    public AudioClip space_track;
    public AudioClip poly_track;
    public AudioClip spook_track;

    private List<AudioClip> sfx_clips;
    private List<AudioClip> music_tracks;

    // AudioManager instance for reference
    public static AudioManager instance;

    private List<AudioSource> sources;

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
        // Init default sfx
        sfx_clips = new List<AudioClip>{
            success_sfx, fail_sfx, activate, interact_sfx
        };

        // Init default tracks
        music_tracks = new List<AudioClip>{
            moon_track, space_track, poly_track, spook_track
        };

        // Create audio sources to play clips from
        track_0 = gameObject.AddComponent<AudioSource>();
        track_1 = gameObject.AddComponent<AudioSource>();

        // Loop music
        track_0.loop = true;
        track_1.loop = true;

        track_0.volume = global_volume * 0.7f;
        track_1.volume = global_volume * 0.7f;

        // Play track_0 on start
        current_track = true;
        SwapTracks(MusicTracks.SPACE);

        // Create an empty audio-sources array
        sources = new List<AudioSource>();
    }

    // Swap between audio sources with a fade effect
    public void SwapTracks(MusicTracks clip, float fadeTime = 1.5f)
    {
        // Stop other coroutines to prevent sounds from getting entertwined
        StopAllCoroutines();

        AudioClip track = music_tracks[(int)clip];

        // Swap the current music track
        if (current_track)
        {
            track_1.clip = track;
            track_1.Play();
            current_track = false;

            // Use a coroutine to fade between tracks
            StartCoroutine(Fade(track_1, track_0, fadeTime));
        }
        else
        {
            track_0.clip = track;
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
            source_1.volume = Mathf.Lerp(0, global_volume, elapsed / fadeTime);
            source_2.volume = Mathf.Lerp(global_volume, 0, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Stop playing music when the volume is 0
        source_2.Stop();
    }

    // Sets global volume
    public void SetVolume(float new_volume)
    {
        global_volume = new_volume;
        track_0.volume = global_volume * 0.7f;
        track_1.volume = global_volume * 0.7f;
        foreach (AudioSource source in sources)
        {
            source.volume = global_volume;
        }
    }

    public void PlayEffect(GameObject game_object, DefaultClips clip)
    {
        float volume_multiplier;
        switch (clip)
        {
            case DefaultClips.INTERACT:
                volume_multiplier = 2.0f;
                break;
            case DefaultClips.ACTIVATE:
                volume_multiplier = 0.7f;
                break;
            default:
                volume_multiplier = 1f;
                break;
        }
            

        PlayEffect(game_object, sfx_clips[(int)clip], volume_multiplier);
    }

    public void PlayEffect(GameObject game_object, AudioClip clip, float volume_multiplier = 1f)
    {
        // Check input validity
        if (clip == null || gameObject == null)
        {
            return;
        }

        AudioSource source = game_object.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = global_volume * volume_multiplier;
        sources.Add(source);
        source.Play();
        StartCoroutine(PlayDestroy(source));
    }

    // Destroy new audiosource on sound end
    private IEnumerator PlayDestroy(AudioSource source)
    {
        while (source.isPlaying)
        {
            if (source != null)
            {
                yield return null;
            }
        }

        sources.Remove(source);
        Destroy(source);
    }
}

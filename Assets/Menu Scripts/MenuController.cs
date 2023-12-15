using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Owen Ludlam
public class MenuController : MonoBehaviour
{
    public AudioClip level_1_track;
    public AudioClip click_sound;
    public AudioSource menu_source;

    // Start Game button reference
    public Button start_button;
    public Button instruction_button;
    public Button exit_button;
    public Canvas main_canvas;
    public Canvas instruction_canvas;
    public Slider volume_slider;
    public string level_1_scene;

    void Start()
    {
        menu_source.clip = click_sound;
        start_button.onClick.AddListener(StartGame);
        instruction_button.onClick.AddListener(ShowInstructions);
        exit_button.onClick.AddListener(HideInstructions);
        instruction_canvas.gameObject.SetActive(false);
        volume_slider.onValueChanged.AddListener(delegate { AudioManager.instance.SetVolume(volume_slider.value); });
    }

    void StartGame()
    {
        StartCoroutine(FinishClick());
    }

    void ShowInstructions()
    {
        main_canvas.gameObject.SetActive(false);
        instruction_canvas.gameObject.SetActive(true);
    }

    void HideInstructions()
    {
        main_canvas.gameObject.SetActive(true);
        instruction_canvas.gameObject.SetActive(false);
    }

    private IEnumerator FinishClick()
    {
        menu_source.volume = AudioManager.instance.global_volume;
        menu_source.Play();

        while (menu_source.isPlaying)
        {
            yield return null;
        }

        AudioManager.instance.SwapTracks(AudioManager.MusicTracks.POLY);
        PauseMenuController.instance.gameObject.SetActive(true); // Allow Pausing
        SceneManager.LoadScene(level_1_scene, LoadSceneMode.Single); // Load single to avoid glitch issues
    }
}

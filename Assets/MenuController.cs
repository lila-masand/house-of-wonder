using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Scripts by Owen Ludlam
public class MenuController : MonoBehaviour
{
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
        SceneManager.LoadScene(start_scene_name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && this.GetComponent<MeshRenderer>().material.GetColor("_Color") == Color.white)
        {
            SceneManager.LoadScene("Level 1");
        }
    }


    

}

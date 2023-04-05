using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
    void Awake()
    {
        GameObject.Destroy(GameObject.Find("GameManagerObject"));
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Dark City");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}

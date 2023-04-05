using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MextMapLoad : MonoBehaviour
{
    public Text pressButtonText;

    private void OnTriggerEnter(Collider other)
    {
        pressButtonText.text = "Press 'P' Button !";

        if(Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("City inside");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("City inside");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pressButtonText.text = " ";
    }
}

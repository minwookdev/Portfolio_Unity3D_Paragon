using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    bool IsPlay;

    public Canvas voice;


    void OnTriggerEnter(Collider other)
    {
       GetComponent<AudioSource>().Play();
       
    }

    void OnTriggerStay(Collider other)
    {
        voice.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        voice.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        voice.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

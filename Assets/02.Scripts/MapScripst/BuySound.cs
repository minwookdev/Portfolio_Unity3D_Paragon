using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySound : MonoBehaviour
{
    public AudioSource BS;
    [SerializeField] private AudioClip clip;

    // Start is called before the first frame update
     void Awake()
    {
        BS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    
    public void PlaySE()
    {
        BS.Play();
    }
}

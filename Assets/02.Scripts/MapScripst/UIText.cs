using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIText : MonoBehaviour
{
    public Canvas UItext;

    // Start is called before the first frame update
    void Start()
    {
        UItext.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PLAYER")
        {
            UItext.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            UItext.enabled = false;
    }
}

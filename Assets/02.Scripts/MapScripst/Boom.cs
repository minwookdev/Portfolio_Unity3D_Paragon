using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PLAYER")
        {
           


        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

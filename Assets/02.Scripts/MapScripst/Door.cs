using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : MonoBehaviour
{


    private Animation anim;

    public GameObject door;


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PLAYER")
        {
            door.GetComponent<Animation>().Play("door");

            
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class STORE : MonoBehaviour
{
    public Canvas store;
    public Canvas pressText;
    
    // Start is called before the first frame update
    void Awake()
    {
        store.enabled = false;
        pressText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void Onclickclose()
    {
        store.enabled = false;
        Time.timeScale = 1;

    }

    private void OnTriggerStay(Collider other)
    { 
        if(other.gameObject.tag == "PLAYER")
        {
            pressText.enabled = true;

            if(Input.GetKey(KeyCode.E))
            {
                store.enabled = true;
                Time.timeScale = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pressText.enabled = false;
    }

    public void UpgradeHealEffect()
    {
        if(GameManager.instance.healEffect < 3 && 
            GameManager.instance.currentGold >= 50)
        {
            GameManager.instance.healEffect += 1;
            GameManager.instance.currentGold -= 50;
        }
    }

    public void OnMagneticEffect()
    {
        if(!GameManager.instance.magneticEffect && 
            GameManager.instance.currentGold >= 250)
        {
            GameManager.instance.magneticEffect = true;
            GameManager.instance.currentGold -= 250;
        }
    }




}

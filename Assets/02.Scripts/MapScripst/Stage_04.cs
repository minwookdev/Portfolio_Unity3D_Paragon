using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_04 : MonoBehaviour
{
    private const string playerTag = "PLAYER";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            //GameManager.instance.stage = GameManager.STAGE.STAGE_04;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseBulletWithObject : MonoBehaviour
{
    private const string playerBulletTag = "BULLET";
    private const string enemyBulletTag = "E_BULLET";

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == playerBulletTag || 
            coll.gameObject.tag == enemyBulletTag)
        {
            Destroy(coll.gameObject);
        }
    }
}

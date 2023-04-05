using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameManager : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Destroy(GameObject.Find("GameManagerObject"));
    }

}

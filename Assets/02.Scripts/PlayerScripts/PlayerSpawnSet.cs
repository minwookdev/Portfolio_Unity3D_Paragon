using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnSet : MonoBehaviour
{
    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //transform.position = GameManager.instance.playerSpawnPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healitem : MonoBehaviour
{
    private float shakeTime = 0.0f;
    private float rotSpeed = 200.0f;
    private float moveSpeed = 10.0f;
    private float heal;

    private Transform playerTr;

    private const string playerTag = "PLAYER";

    void Awake()
    {
        if (GameManager.instance.healEffect == 0)
        {
            heal = 8.0f;
        }
        else if (GameManager.instance.healEffect == 1)
        {
            heal = 10.0f;
        }
        else if (GameManager.instance.healEffect == 2)
        {
            heal = 12.0f;
        }
        else if (GameManager.instance.healEffect == 3)
        {
            heal = 15.0f;
        }
    }

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Magetic();

        shakeTime += Time.deltaTime;

        if (shakeTime < 1.0f)
        {
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
        else if (shakeTime > 1.0f)
        {
            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        }

        if (shakeTime > 2.0f)
        {
            shakeTime = 0.0f;
        }

        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0), Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == playerTag)
        {
            var playerstatus = other.gameObject.GetComponent<PlayerStatus>();
            playerstatus.currentHp += heal;
            playerstatus.UpdateHpInfo();
            playerstatus.DisplayHpBar();
            Destroy(this.gameObject);
        }
    }

    void Magetic()
    {
        if (!GameManager.instance.magneticEffect) return;

        if (playerTr != null)
        {
            if (Vector3.Distance(playerTr.position, transform.position) < 5.0f)
            {
                transform.position += (playerTr.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            }
        }
    }
}

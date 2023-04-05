using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage01_Start : MonoBehaviour
{
    private const string playerTag = "PLAYER";
    private Collider collider;
    private float gameTime = 0.0f;
    private bool gameStart = false;

    public Collider nextStage;
    public TextMeshPro nextText;
    public TextMeshPro stageText;
    public Text timeLeft;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();

        timeLeft.text = " ";
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == playerTag)
        {
            collider.isTrigger = false;
            nextStage.isTrigger = false;
            GameManager.instance.stage = GameManager.STAGE.STAGE_01;

            gameStart = true;
        }
    }
    
    private void Update()
    {
        if(gameStart)
        {
            gameTime += Time.deltaTime;
            nextText.text = "Survive On This Stage";
            stageText.text = "Survive On This Stage";

            timeLeft.text = "( 60sec / " + Mathf.Round(gameTime).ToString() + "sec )";
        }

        if(gameTime > 60.0f && gameStart)
        {
            nextStage.isTrigger = true;
            collider.isTrigger = true;
            GameManager.instance.DestroyEnemy();
            GameManager.instance.stage = GameManager.STAGE.NONE_STAGE;

            nextText.text = "STAGE 02 START";
            stageText.text = "STAGE 01 START";
            timeLeft.text = "";
            //GameManager.instance.stage = GameManager.STAGE.STAGE_02;
            //Destroy(this.gameObject);
            gameTime = 0.0f;
            gameStart = false;
        }
    }
}

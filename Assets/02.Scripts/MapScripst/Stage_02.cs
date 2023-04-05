using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage_02 : MonoBehaviour
{
   
    private const string playerTag = "PLAYER";

    private Collider collider;

    public Collider nextStage;

    private float gameTime = 0.0f;
    private bool gameStart = false;

    public TextMeshPro stageText;
    public TextMeshPro nextStageText;
    public Text timeText;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            collider.isTrigger = false;
            nextStage.isTrigger = false;
            GameManager.instance.stage = GameManager.STAGE.STAGE_02;

            stageText.text = "Survive On This Stage";
            nextStageText.text = "Survive On This Stage";

            gameStart = true;
        }
    }

    private void Update()
    {
        if (gameStart)
        {
            gameTime += Time.deltaTime;
            timeText.text = "( 60sec / " + Mathf.Round(gameTime).ToString() + "sec )";
        }

        if (gameTime > 60.0f && gameStart)
        {
            GameManager.instance.DestroyEnemy();
            nextStage.isTrigger = true;

            stageText.text = "STAGE 02 START";
            nextStageText.text = "STAGE 03 START";
            timeText.text = "";

            GameManager.instance.stage = GameManager.STAGE.NONE_STAGE;
            gameTime = 0;
            gameStart = false;
            //GameManager.instance.stage = GameManager.STAGE.STAGE_03;

            //Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage_03 : MonoBehaviour
{
    private const string playerTag = "PLAYER";

    private Collider collider;

    public Collider nextStage;

    public TextMeshPro stageText;
    public TextMeshPro nextStageText;
    public Text timeText;

    private float gameTime = 0.0f;
    private bool gameStart = false;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == playerTag)
        {
            collider.isTrigger = false;
            nextStage.isTrigger = false;

            stageText.text = "Survive On This Stage";
            nextStageText.text = "Survive On This Stage";

            gameStart = true;

            GameManager.instance.stage = GameManager.STAGE.STAGE_03;
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
 
            nextStage.isTrigger = true;
            GameManager.instance.DestroyEnemy();

            stageText.text = "STAGE 03 START";
            nextStageText.text = "INSIDE DARK CITY";
            timeText.text = "Go To DarkCity Inside";

            gameTime = 0;
            gameStart = false;
            GameManager.instance.stage = GameManager.STAGE.NONE_STAGE;
            //GameManager.instance.stage = GameManager.STAGE.STAGE_04;
            //Destroy(this.gameObject);
        }
    }
}

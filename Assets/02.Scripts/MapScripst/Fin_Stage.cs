using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fin_Stage    : MonoBehaviour
{
    private const string playerTag = "PLAYER";
    private Collider collider;
    private float gameTime = 0.0f;
    private bool gameStart = false;
    public Text timeText;

    public TextMeshPro stageText;
    public TextMeshPro nextStageText;


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
            //nextStage.isTrigger = false;
            GameManager.instance.stage = GameManager.STAGE.Fin_Stage;

            gameStart = true;
        }
    }

    private void Update()
    {
        if (gameStart)
        {
            gameTime += Time.deltaTime;
            nextStageText.text = "Survive On This Place";
            stageText.text = "Survive On This Place";
            timeText.text = "( 120sec / " + Mathf.Round(gameTime).ToString() + "sec )";

        }

        if (gameTime > 120.0f && gameStart)
        {
            //nextStage.isTrigger = true;
            //collider.isTrigger = true;
            GameManager.instance.DestroyEnemy();
            GameManager.instance.stage = GameManager.STAGE.NONE_STAGE;
            timeText.text = "";
            nextStageText.text = "START";
            stageText.text = "START";

            //GameManager.instance.stage = GameManager.STAGE.STAGE_02;
            //Destroy(this.gameObject);
            gameTime = 0.0f;
            gameStart = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerControll))]

public class PlayerStatus : MonoBehaviour
{
    private const string enemyBulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";

    private Animator anim;
    private readonly int hashDeath = Animator.StringToHash("isDead");

    [HideInInspector]
    public float maxHp = 250.0f;
    [HideInInspector]
    public float currentHp;

    public Image hpBar;
    public Text hpInfo;
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currentColor;

    private PlayerControll pControll;

    private CapsuleCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        pControll = GetComponent<PlayerControll>();
        anim = GetComponent<Animator>();

        hpBar.color = initColor;
        currentColor = initColor;

        if(GameManager.instance.playerHpGrade == 0)
        {
            maxHp = 250;
        }
        else if(GameManager.instance.playerHpGrade == 1)
        {
            maxHp = 300;
        }
        else if (GameManager.instance.playerHpGrade == 2)
        {
            maxHp = 350;
        }

        currentHp = maxHp;
        UpdateHpInfo();
    }

    void Update()
    {
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
            UpdateHpInfo();
            DisplayHpBar();
        }

        if(currentHp <= 0)
        {
            currentHp = 0;
            UpdateHpInfo();
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == enemyBulletTag)
        {
            currentHp -= coll.gameObject.GetComponent<EnemyBullet>().damage;
            Destroy(coll.gameObject);

            //생명게이지 크기및 수치 변경 함수 호출
            DisplayHpBar();
            //피격시 마다 체력 Text Update
            UpdateHpInfo();

            if (currentHp <= 0 && pControll.isDead == false)
            {
                pControll.isDead = true;
                OnPlayerDie();
            }
        }
    }

    void OnPlayerDie()
    {
        anim.SetTrigger(hashDeath);

        collider.height = 0.1f;
        collider.center = new Vector3(0.0f, 0.2f, 0.0f);

        GameManager.instance.isGameOver = true;
    }

    public void DisplayHpBar()
    {
        //생명수치가 절반 일 때 녹색에서 노란색으로 변경
        if((currentHp / maxHp) > 0.5f)
        {
            currentColor.r = (1 - (currentHp / maxHp)) * 2;
        }
        else //노란색에서 빨간색으로 으로 변경
        {
            currentColor.g = (currentHp / maxHp) * 2.0f;
        }

        //hpBar의 색상과 크기를 변경합니다.
        hpBar.color = currentColor;
        hpBar.fillAmount = (currentHp / maxHp);
    }

    public void UpdateHpInfo()
    {
        hpInfo.text = string.Format("{0} / {1}", currentHp, maxHp);
    }

    public void UpgradeHpMax()
    {
        if (GameManager.instance.playerHpGrade == 2) return;

        maxHp += 50;
        currentHp = maxHp;
        UpdateHpInfo();
        DisplayHpBar();
        GameManager.instance.playerHpGrade += 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";
    //생명 게이지 
    public float hp = 150.0f;
    //초기 생명 수치 
    //private float initHp = 150.0f;
    private bool isDie = false;

    // 피격시 사용할 효고 ( 이팩트 )
    private GameObject HitEffect;

    private int sniperRifleMinDamage = -3;
    private int sniperRifleMaxDamage = 3;

    private Enemy enemy;
    public GameObject sniperRifleEffect;

    public GameObject hudDamageText;
    public Transform hudPos;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    public GameObject healItem;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "BULLET")
        {
            var bullet = coll.gameObject.GetComponent<BulletControll>();
            
            if(bullet.isCritical)
            {
                TakeDamage(bullet.bulletDamage, Color.yellow);
            }

            else if (!bullet.isCritical)
            {
                TakeDamage(bullet.bulletDamage, Color.white);
            }

            //생명게이지 차감 
            hp -= bullet.bulletDamage;
            //총알 삭제 
            Destroy(coll.gameObject);

            if (hp <= 0.0f)
            {
                int itemDropChance = Random.Range(0, 100);

                if (itemDropChance >= 50)
                {
                    Instantiate(healItem, coll.transform.position, coll.transform.rotation);
                }

                enemy.EnemyState = Enemy.CurrentState.Die;
                GameManager.instance.currentGold += 10;
                //적 캐릭터가 사망한 이후 생명 게이지를 투명 처러ㅣ 
                //hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }
        }
    }
    void ShowHitEffect(Collision coll)
    {
        //총알이 충돌한 지점을 알아야함.
        Vector3 pos = coll.contacts[0].point;
        Vector3 _normal = coll.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
    }

    void OnDamage(object[] _infos)
    {

        float sniperDamage = (float)_infos[1];
        sniperDamage = (int)sniperDamage;
        sniperDamage += Random.Range(sniperRifleMinDamage, sniperRifleMaxDamage);

        //currentHp -= (float)_infos[1];

        Debug.Log(sniperDamage.ToString());

        hp -= sniperDamage;

        TakeDamage(sniperDamage, Color.red);

        CreateSniperRifleEffect((Vector3)_infos[0]);

        if (hp <= 0)
        {
            enemy.EnemyState = Enemy.CurrentState.Die;
        }
    }

    void CreateSniperRifleEffect(Vector3 pos)
    {
        GameObject effect = (GameObject)Instantiate(sniperRifleEffect, pos, Quaternion.identity) as GameObject;
        float particleTime = effect.GetComponent<ParticleSystem>().duration;
        Destroy(effect, particleTime);
    }

    public void TakeDamage(float Damage, Color color)
    {
        GameObject hudText = Instantiate(hudDamageText) as GameObject;         //생성할 텍스트 오브젝트
        hudText.transform.position = hudPos.position;                          //Damage 수치가 표시될 위치.
        hudText.GetComponent<DamageCount>().damage = Damage;
        hudText.GetComponent<DamageCount>().criticalColor = color;
    }

    void PlayerDie()
    {
        OnPlayerDie();
        
    }

}

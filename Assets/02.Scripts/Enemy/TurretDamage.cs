using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurretDamage : MonoBehaviour
{


    private const string bulletTag = "E_BULLET";
    private const string enemyTag = "ENEMY";
    //생명 게이지 
    private float hp = 200.0f;
    //초기 생명 수치 
    //private float initHp = 150.0f;
    private bool isDie = false;

    // 죽었을 때  사용할 이팩트
    public GameObject dieEffect;
    public GameObject t_DieEffect;
    public Transform EffectPos;
    public Transform dieEffectPos;
    private int sniperRifleMinDamage = -3;
    private int sniperRifleMaxDamage = 3;
    private bool isSmoke = false;   //연기 이펙트 출력

    private Enemy enemy;
    public GameObject sniperRifleEffect;

    public GameObject hudDamageText;
    public Transform hudPos;

    void Start()
    {
        enemy = GetComponent<Enemy>();

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            var bullet = coll.gameObject.GetComponent<BulletControll>();

           //총알 삭제 
           Destroy(coll.gameObject);
            
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

            if (hp <= 100.0f && !isSmoke)
            {
                turretDieEffect(coll.transform.position);
                isSmoke = true;
            }

            if (hp <= 0.0f)
            {
                T_DieEffect(coll.transform.position);
                Destroy(this.gameObject);
            }
        }
    }

    void OnDamage(object[] _infos)
    {

        float sniperDamage = (float)_infos[1];
        sniperDamage = (int)sniperDamage;
        sniperDamage += Random.Range(sniperRifleMinDamage, sniperRifleMaxDamage);

        hp -= sniperDamage;

        TakeDamage(sniperDamage, Color.red);

        CreateSniperRifleEffect((Vector3)_infos[0]);
        if (hp <= 100.0f && !isSmoke)
        {
            turretDieEffect(transform.position);
            isSmoke = true;
        }
        if (hp <= 0)
        {
            T_DieEffect(transform.position);
            Destroy(this.gameObject);
            //enemy.EnemyState = Enemy.CurrentState.Die;
        }
    }

    void turretDieEffect(Vector3 pos)
    {
        GameObject t_Effect = (GameObject)Instantiate(dieEffect, pos, Quaternion.identity) as GameObject;
        //float effectTime = t_Effect.GetComponent<ParticleSystem>().duration;
        float effectTime = 10.0f;
        Destroy(t_Effect, effectTime);
    }

    void T_DieEffect(Vector3 pos)
    {
        GameObject T_Effect = (GameObject)Instantiate(t_DieEffect, pos, Quaternion.identity) as GameObject;
        float effectTime = T_Effect.GetComponent<ParticleSystem>().duration;
        Destroy(T_Effect, effectTime);
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

   
}

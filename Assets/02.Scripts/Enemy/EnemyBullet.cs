using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //총알 뎀지 
    public float damage = 0.0f;
    //총알 발사 속도 
    public float speed = 0.0f;
    private const string playerTag = "PLAYER";
    private const string wallTag = "WALL";

    //스파크 프리팹을 저장할 변수 
    public GameObject sparkEffect;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    //충돌이 시작할 때 발생하는 이벤트 
    private void OnCollisionEnter(Collision coll)
    {
        //충돌한 게임 오브젝트의 태그값 비교 
        if (coll.gameObject.tag == playerTag)
        {
            //스파크 효과 함수 호출
            ShowEffect(coll);
            Destroy(this.gameObject);
           
        }
        else if (coll.gameObject.tag == wallTag)
        {
            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        //Destroy(this.gameObject, dest)
    }

    void ShowEffect(Collision coll)
    {
        //충돌 지점의 정보를 추출 
        ContactPoint contact = coll.contacts[0];
        //법선 벡터가 이루는 회전각도를 추출 
        //Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        //스파크 효과를 생성 
        GameObject sp = Instantiate(sparkEffect, contact.point, Quaternion.identity) as GameObject;
        float effectTime = sp.GetComponent<ParticleSystem>().duration; 
        Destroy(sp, effectTime);
    }
}

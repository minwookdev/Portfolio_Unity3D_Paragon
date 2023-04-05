using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    //총알 뎀지 
    public  float damage = 10.0f;
    //총알 발사 속도 
    public float speed = 800.0f;
    private const string playerTag = "PLAYER";
    private const string wallTag = "WALL";

    public GameObject hitEffect;

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
            Effect(coll);

            //충돌한 게임오브젝트 삭제 
            //Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == wallTag)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {

    }

    void Effect(Collision coll)
    {
        //충돌 지점의 정보를 추출 
        ContactPoint contact = coll.contacts[0];
        //법선 벡터가 이루는 회전각도를 추출 
        //Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        //스파크 효과를 생성 
        GameObject Spack = Instantiate(hitEffect, contact.point, Quaternion.identity) as GameObject;
        float effectTime = Spack.GetComponent<ParticleSystem>().duration;
        Destroy(Spack, effectTime);
    }
}

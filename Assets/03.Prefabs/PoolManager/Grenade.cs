using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody rigid;
    private Collider collider;
    private Transform tr;
    public GameObject expEffect;
    private Light sphereLight;

    private float fireSpeed = 500.0f;
    private float count = 0.0f;

    private void Awake()
    {
        //컴포넌트 할당.
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        tr = GetComponent<Transform>();
        sphereLight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        //활성화되면 forward방향으로 발사
        rigid.AddForce(tr.forward * fireSpeed);
    }

    private void OnDisable()
    {
        count = 0.0f;

        //비활성화되면 여러 값을 초기화했습니다.
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rigid.Sleep();
    }

    void Update()
    {
        count += Time.deltaTime;

        if(count > 3.0)
        {
            StartCoroutine(GrenadeExp());

        }
    }

    IEnumerator GrenadeExp()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity) as GameObject;
        float dur = effect.GetComponent<ParticleSystem>().duration;
        Destroy(effect, dur);

        this.gameObject.SetActive(false);
    }
}

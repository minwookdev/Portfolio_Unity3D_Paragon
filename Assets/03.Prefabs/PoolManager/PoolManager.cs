using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //게임오브젝트와 마찬가지로 싱글톤화하여 사용했습니다.
    public static PoolManager instance = null;

    [Header("Bullet Pool")]
    //Bullet Object Pooling 부분입니다.
    public GameObject bullet;
    public int maxBullet = 5;
    //최대 생성할 Bullet갯수와, Bullet GameObject를 담을 List 생성했습니다.
    public List<GameObject> bulletPool = new List<GameObject>(); 

    void Awake()
    {
        //if(instance == null)
        //{
        //    instance = this;
        //}
        //else if (instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //
        //DontDestroyOnLoad(this.gameObject);
        //
        ////시작되고 BulletPool만들었습니다.
        //CreateBulletPool();

        instance = this;
    }

    //오브젝트 풀에 총알을 생성할 함수입니다.
    //(실행되고 한번 만 실행될 함수)
    public void CreateBulletPool()
    {
        //BulletPool을 생성하고 차일드화 할 때, 이들의 부모가 될 Parent GameObject
        GameObject bulletPools = new GameObject("BulletPools");

        //생성할 최대 갯수만큼 미리 총알을 생성했습니다.
        for (int i = 0; i < maxBullet; i++)
        {
            //MaxBullet 만큼 생성하고, 비활성화 해줍니다.
            var poolingObject = Instantiate(bullet, bulletPools.transform);
            poolingObject.name = "Bullet_" + i.ToString();
            poolingObject.SetActive(false);

            //생성된 총알을 bulletPool List에 담아줍니다.
            bulletPool.Add(poolingObject);
        }
    }

    //BulletPool에서 놀고있는 Bullet 불러옵니다.
    //Instantiate가 아닌 PoolManager에서 이 함수를 호출하여 발사하도록 했습니다.
    public GameObject GetBullet()
    {
        for(int i =0;i<bulletPool.Count;i++)
        {
            if(bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }

        return null;
    }
}

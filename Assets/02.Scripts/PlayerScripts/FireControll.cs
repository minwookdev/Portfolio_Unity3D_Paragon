using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
}

[RequireComponent(typeof(PlayerControll))]

public class FireControll : MonoBehaviour
{
    public enum WEAPONTYPE
    {
        NONE_WEAPON,
        ASSULT_RIFLE,
        SHOT_GUN,
        SNIPER_RIFLE,
        GRENADE,
    }

    [Header("Fire Controll")]
    public WEAPONTYPE weaponType = WEAPONTYPE.ASSULT_RIFLE;     //WEAPON TYPE
    public GameObject bulletPrefabs;                            //Rifle Bullet 프리팹 저장.
    public GameObject shotGunPrefabs;                           //Shot gun Prefabs.
    public Transform firePos;                                   //총알 발사 위치
    public bool isAiming = false;                               //조준모드
    public bool isRoll = false;                                 //회피
    private bool isReloading = false;                           //재장전
    public Text currentWeapon;                                  //현재 무기 종류 UI Text
    public Text ammoInfo;                                       //장탄수 정보 UI Text
    public ParticleSystem muzzleFlash;                          //총구 화염효과 Effect;
    private AudioSource _audio;                                 //Audio Component
    public PlayerSfx playerSfx;

    [Header("ASSULT RIFLE")]
    public float bulletSpeed = 3000.0f;                         //Bullet Speed
    public float bulletDamage = 10.0f;                          //Bullet Damage
    public int maxRifleAmmo = 30;                               //최대 돌격소총의 장탄
    public int currRifleAmmo;                                   //현재 돌격소총의 장탄
    private float rayDistance = 100.0f;                         //RayCast 발사 거리
    private float rapidFire = 0.0f;
    private float fireDelay = 0.1f;                              //사격 딜레이

    [Header("SHOT GUN")]
    public int pelletCount = 5;                                 //발사될 총알 갯수
    public float spreadAngle = 10.0f;                           //총알의 퍼짐 각도
    public float shotGunBulletSpeed = 1000.0f;                  //산탄총 탄속
    public int maxShotAmmo = 8;                                 //최대 산탄총 장탄
    public int currShotAmmo;                                    //현재 산탄총 장탄
    List<Quaternion> pellets;                                   //랜덤한 각도를 담을 List

    [Header("SNIPER RIFLE")]
    public float sniperRifleDamage = 80.0f;                     //스나이퍼 라이플 대미지
    public int maxSniperAmmo = 3;                               //최대 저격총 장탄
    public int currSniperAmmo;                                  //현재 저격총 장탄

    [Header("GRANADE")]
    public int maxGrenade = 3;                                  //최대 유탄장전량
    public int currGrenade = 0;                                 //현재 유탄장전량

    [Header("UPGRADE")]
    public int currRiflePowerGrade = 0;                             //라이플파워 강화수치.
    public int currRifleAmmoGrade = 0;                              //라이플장탄 강화수치.
    public int currShotPowerGrade = 0;                              //산탄총파워 강화수치.
    public int currShotAmmoGrade = 0;                               //산탄총장탄 강화수치.
    public int currSniperPowerGrade = 0;                            //저격총파워 강화수치.
    public int currSniperAmmoGrade = 0;                             //저격총장탄 강화수치.

    private LineRenderer laserPointer;
    private PlayerControll pControll;
    private Animator anim;
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    // Start is called before the first frame update
    void Awake()
    {
        pellets = new List<Quaternion>(pelletCount);

        for (int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }

        //처음 무기 타입 지정
        weaponType = WEAPONTYPE.ASSULT_RIFLE;

        _audio = GetComponent<AudioSource>();

        //스나이퍼 라이플의 레이저포인터 설정.
        laserPointer = GetComponent<LineRenderer>();
        laserPointer.SetColors(Color.red, Color.yellow);
        laserPointer.SetWidth(0.025f, 0.025f);
        laserPointer.enabled = false;

        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //GameManager에서 대미지, 장탄강화수치 받아오기.
        currRiflePowerGrade = GameManager.instance.rifleGrade;
        currShotPowerGrade = GameManager.instance.shotGrade;
        currSniperPowerGrade = GameManager.instance.sniperGrade;
        currRifleAmmoGrade = GameManager.instance.rifleAmmoGrade;
        currShotAmmoGrade = GameManager.instance.shotAmmoGrade;
        currSniperAmmoGrade = GameManager.instance.sniperAmmoGrade;

        Debug.Log("POWER : " + currRiflePowerGrade.ToString() + " , " + currShotPowerGrade.ToString() + " , " + currSniperPowerGrade.ToString() +
                   " AMMO : " + currRifleAmmoGrade.ToString() + " , " + currShotAmmoGrade.ToString() + " , " + currSniperAmmoGrade.ToString());

        //업그레이드 횟수와 맞춘다.
        CheckAmmoGrade();

        //장탄수 설정.
        currRifleAmmo = maxRifleAmmo;
        currShotAmmo = maxShotAmmo;
        currSniperAmmo = maxSniperAmmo;
        currGrenade = maxGrenade;

        UpdateWeaponInfo();
        UpdateAmmoInfo();

        pControll = GetComponent<PlayerControll>();
    }

    private void Update()
    {
        if (pControll.isDead) return;

        //조준모드
        if (isAiming && !isRoll)
        {
            if(weaponType == WEAPONTYPE.ASSULT_RIFLE)
            {
                laserPointer.enabled = false;

                if(Input.GetMouseButton(0) && !isReloading)
                {
                    rapidFire += Time.deltaTime;
                
                    if(rapidFire >= fireDelay)
                    {
                        --currRifleAmmo;
                        BulletFire();
                        rapidFire = 0.0f;
                        muzzleFlash.startRotation = firePos.position.y;
                        muzzleFlash.Play();
                        FireSfx(1);
                    }
                }

                if(currRifleAmmo == 0)
                {
                    StartCoroutine(this.Reload());

                    //anim.SetTrigger(hashReload);
                }
            }

            else if (weaponType == WEAPONTYPE.SHOT_GUN)
            {
                laserPointer.enabled = false;

                if(Input.GetMouseButtonDown(0) && !isReloading)
                {
                    ShotGunFire();
                    anim.SetTrigger(hashFire);
                    --currShotAmmo;
                    UpdateAmmoInfo();
                    FireSfx(2);

                    muzzleFlash.startRotation = firePos.position.y;
                    muzzleFlash.Play();
                }

                if(currShotAmmo == 0)
                {
                    StartCoroutine(this.Reload());

                    //anim.SetTrigger(hashReload);
                }
            }

            if(weaponType == WEAPONTYPE.SNIPER_RIFLE)
            {
                RaycastHit[] hits;

                hits = Physics.RaycastAll(firePos.position, firePos.forward, rayDistance);

                //RayHit와 정확도 부정확할 수 있습니다.
                //완벽하게 정확도 맞추려면 for문 아래에 lasetPointer 주석풀고 그거쓰면 됩니다.
                //laserPointer.enabled = true;
                //laserPointer.SetPosition(0, firePos.position);
                //laserPointer.SetPosition(1, firePos.forward * rayDistance);

                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];

                    Debug.DrawRay(firePos.position, firePos.forward);

                    laserPointer.enabled = true;
                    laserPointer.SetPosition(0, firePos.position);
                    laserPointer.SetPosition(1, hit.point + firePos.forward * rayDistance);
                    
                    if (Input.GetMouseButtonDown(0) && !isReloading)
                    {
                        if (hit.collider.tag == "ENEMY")
                        {
                            //int randomCritical = Random.Range(1, 11);

                            //스나이퍼 라이플 치명타 확률 랜덤.
                            //스나이퍼 라이플 치명타 방법 생각해보기.
                            //if(randomCritical <= sniperCriticalChace)
                            //{
                            //    isSniperCritical = true;
                            //}
                            //else
                            //{
                            //    isSniperCritical = false;
                            //}

                            object[] _infos = new object[2];
                            _infos[0] = hit.point;              //Ray에 맞은 위치값.
                            //_infos[1] = sniperRifleDamage;      //Enemy 에 전달할 대미지 값.

                            switch(currSniperPowerGrade)
                            {
                                case 0:
                                    _infos[1] = sniperRifleDamage;
                                    break;
                                case 1:
                                    _infos[1] = sniperRifleDamage + 30.0f;
                                    break;
                                case 2:
                                    _infos[1] = sniperRifleDamage + 60.0f;
                                    break;
                            }
                    
                            hit.collider.gameObject.SendMessage("OnDamage",
                                _infos,
                                SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }

                if(Input.GetMouseButtonDown(0) && !isReloading)
                {
                    //sniperRifleDamage += Random.Range(sniperMinDamage, sniperMaxDamage);
                    --currSniperAmmo;
                    UpdateAmmoInfo();
                    FireSfx(3);

                    muzzleFlash.startRotation = firePos.position.y;
                    muzzleFlash.Play();
                }

                if(currSniperAmmo == 0)
                {
                    StartCoroutine(this.Reload());

                    //anim.SetTrigger(hashReload);
                }
            }

            else if(weaponType == WEAPONTYPE.GRENADE)
            {
                if(Input.GetMouseButtonDown(0) && !isReloading)
                {
                    if(currGrenade > 0)
                    {
                        --currGrenade;
                        StartCoroutine(this.GrenadeFire());
                    }

                }
            }
        }
        if(!isAiming)
        {
            laserPointer.enabled = false;
        }

        ChangeWeapon();
    }

    void BulletFire()
    {
        RaycastHit _hit;
        
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(_ray, out _hit);
        
        Vector3 bulletPoint = new Vector3(_hit.point.x, firePos.position.y, _hit.point.z);
        
        GameObject bullet = Instantiate(bulletPrefabs, firePos.position, firePos.rotation) as GameObject;
        bullet.transform.LookAt(bulletPoint);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed);

        switch(currRiflePowerGrade)
        {
            case 0:
                bullet.GetComponent<BulletControll>().bulletDamage = 10.0f;
                break;

            case 1:
                bullet.GetComponent<BulletControll>().bulletDamage = 30.0f;
                break;

            case 2:
                bullet.GetComponent<BulletControll>().bulletDamage = 50.0f;
                break;
        }

        anim.SetTrigger(hashFire);

        UpdateAmmoInfo();
    }

    void ShotGunFire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            pellets[i] = Random.rotation;
        
            GameObject shot = Instantiate(shotGunPrefabs, firePos.position, firePos.rotation) as GameObject;
            shot.transform.rotation = Quaternion.RotateTowards(shot.transform.rotation, pellets[i], spreadAngle);
            shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * shotGunBulletSpeed);

            switch (currShotPowerGrade)
            {
                case 0:
                    shot.GetComponent<BulletControll>().bulletDamage = 30.0f;
                    break;

                case 1:
                    shot.GetComponent<BulletControll>().bulletDamage = 50.0f;
                    break;

                case 2:
                    shot.GetComponent<BulletControll>().bulletDamage = 70.0f;
                    break;
                default:
                    break;
            }
        }

        anim.SetTrigger(hashFire);

    }

    void SniperRifle()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(firePos.position, firePos.forward, rayDistance);

        for(int i =0;i<hits.Length;i++)
        {
            RaycastHit hit = hits[i];

            if(hit.collider.tag == "ENEMY")
            {
                object[] _infos = new object[2];
                _infos[0] = hit.point;               //Ray에 맞은 위치값.
                _infos[1] = sniperRifleDamage;       //Enemy 에 전달할 대미지 값.

                hit.collider.gameObject.SendMessage("OnDamage",
                    _infos,
                    SendMessageOptions.DontRequireReceiver);

            }
        }
    }

    IEnumerator Reload()
    {
        anim.SetTrigger(hashReload);
        UpdateAmmoInfo();

        FireSfx(0);

        isReloading = true;

        currRifleAmmo = maxRifleAmmo;
        currShotAmmo = maxShotAmmo;
        currSniperAmmo = maxSniperAmmo;

        yield return new WaitForSeconds(1.2f);



        UpdateAmmoInfo();

        isReloading = false;
    }

    void UpdateWeaponInfo()
    {
        if (weaponType == WEAPONTYPE.ASSULT_RIFLE)
        {
            currentWeapon.text = "ASSULT RIFLE";
            currentWeapon.color = Color.yellow;
        }

        else if (weaponType == WEAPONTYPE.SHOT_GUN)
        {
            currentWeapon.text = "SHOT GUN";
            currentWeapon.color = Color.red;
        }

        else if (weaponType == WEAPONTYPE.SNIPER_RIFLE)
        {
            currentWeapon.text = "SNIPER RIFLE";
            currentWeapon.color = Color.green;
        }

        else if (weaponType == WEAPONTYPE.GRENADE)
        {
            currentWeapon.text = "GRENADE";
            currentWeapon.color = Color.cyan;
        }
    }

    void ChangeWeapon()
    {
        if (isReloading) return;

        //무기교체
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = WEAPONTYPE.ASSULT_RIFLE;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponType = WEAPONTYPE.SHOT_GUN;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponType = WEAPONTYPE.SNIPER_RIFLE;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponType = WEAPONTYPE.GRENADE;
            UpdateWeaponInfo();
            UpdateAmmoInfo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(this.Reload());
        }
    }

    public void UpdateAmmoInfo()
    {
        if(weaponType == WEAPONTYPE.ASSULT_RIFLE)
        {
            ammoInfo.text = string.Format("<color=#ff0000>{0} </color>/ {1}", currRifleAmmo, maxRifleAmmo);
        }

        else if (weaponType == WEAPONTYPE.SHOT_GUN)
        {
            ammoInfo.text = string.Format("<color=#ff0000>{0} </color>/ {1}", currShotAmmo, maxShotAmmo);
        }

        else if (weaponType == WEAPONTYPE.SNIPER_RIFLE)
        {
            ammoInfo.text = string.Format("<color=#ff0000>{0} </color>/ {1}", currSniperAmmo, maxSniperAmmo);
        }

        else if (weaponType == WEAPONTYPE.GRENADE)
        {
            ammoInfo.text = string.Format("<color=red>{0}</color>", currGrenade);
        }
    }

    void FireSfx(int soundNum)
    {
        var _sfx = playerSfx.fire[soundNum];
        //사운드 발생
        _audio.PlayOneShot(_sfx, 1.0f);
    }

    void CheckAmmoGrade()
    {
        //실행될때 현재 장탄업그레이드 횟수 체크해서 장탄수 적용해주는함수.

        switch(currRifleAmmoGrade)
        {
            case 0:
                maxRifleAmmo = 30;
                break;
            case 1:
                maxRifleAmmo = 40;
                break;
            case 2:
                maxRifleAmmo = 50;
                break;
        }

        switch(currShotAmmoGrade)
        {
            case 0:
                maxShotAmmo = 8;
                break;
            case 1:
                maxShotAmmo = 10;
                break;
            case 2:
                maxShotAmmo = 12;
                break;

        }

        switch(currSniperAmmoGrade)
        {
            case 0:
                maxSniperAmmo = 3;
                break;
            case 1:
                maxSniperAmmo = 4;
                break;
            case 2:
                maxSniperAmmo = 5;
                break;
        }
    }

    IEnumerator GrenadeFire()
    {
        FireSfx(4);

        UpdateAmmoInfo();
        isReloading = true;

        //생성하는 방식이 아닌 ObjectPool 에서 가져오는 방식입니다.
        var grenade = PoolManager.instance.GetBullet();
        if(grenade != null)
        {
            //생성될 위치과 회전값
            grenade.transform.position = firePos.position;
            grenade.transform.rotation = firePos.rotation;
            //활성화
            grenade.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        isReloading = false;
    }
}

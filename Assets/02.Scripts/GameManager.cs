using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //STAGE enum
    public enum STAGE
    {
        NONE_STAGE,
        STAGE_01,
        STAGE_02,
        STAGE_03,
        STAGE_04,
        STAGE_05,
        STAGE_06,
        STAGE_07,
        STAGE_08,
        Fin_Stage    
    }

    public STAGE stage = STAGE.NONE_STAGE;

    //GameManager Singleton 
    public static GameManager instance = null;
    //Enemy Spawn Setting
    private Transform[] points;
    private Transform[] points2;
    private Transform[] points3;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject turret;

    //생성 시간 
    private float createEnemy = 10.0f;
    private float createEnemy2 = 15.0f;
    private float createTurret = 3.0f;
    //생성 갯수 
    public int maxEnemy = 0;
    public int maxEnemy2 = 0;
    public int maxTurret = 0;

    //Player의 현재 무기강화 정보를 넘겨줍니다.
    //플레이어의 1번무기 그레이드
    public int rifleGrade = 0;      
    public int rifleAmmoGrade = 0;
    //플레이어의 2번무기 그레이드
    public int shotGrade = 0;       
    public int shotAmmoGrade = 0;
    //플레이어의 3번무기 그레이드
    public int sniperGrade = 0;     
    public int sniperAmmoGrade = 0;
    //플레이어의 체력 업그레이드 
    public int playerHpGrade = 0;
    public int currentGold = 500;
    private Text goldText;
    public bool isGameOver = false;
    public bool magneticEffect = false;
    public int healEffect = 0;

    void Start()
    {
        
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //instance 에 할당된 클래스의 인스턴스가 다를경우 새로 생성된 클래스를 의미합니다.
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        //다른 씬으로 넘어가더라도 삭제하지 않고 유지합니다.
        DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {
        MapChange();

        if(!isGameOver)
        goldText = GameObject.Find("CurrentGold").GetComponent<Text>();

        UpdateCurrentGold();

        switch (stage)
        {
            case STAGE.STAGE_01:

                points = GameObject.Find("EnemySpawn").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("EnemySpawn2").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("TurretSpawn").GetComponentsInChildren<Transform>();
                StartCoroutine(this.CreateEnemy(0));
                StartCoroutine(this.CreateEnemy2(0));
                StartCoroutine(this.CreateTurret());

                break;

            case STAGE.STAGE_02:

                points = GameObject.Find("E_Spawn1").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("E_Spawn2").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("T_Spawn1").GetComponentsInChildren<Transform>();
                
                StartCoroutine(this.CreateEnemy(1));
                StartCoroutine(this.CreateEnemy2(1));
                StartCoroutine(this.CreateTurret());
                
                break;

            case STAGE.STAGE_03:

                points = GameObject.Find("EnemySpawn0").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("EnemySpawn1").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("TurretSpawn1").GetComponentsInChildren<Transform>();
                
                StartCoroutine(this.CreateEnemy(2));
                StartCoroutine(this.CreateEnemy2(2));
                StartCoroutine(this.CreateTurret());


                break;

            case STAGE.STAGE_04:

                break;

            case STAGE.STAGE_05:

                points = GameObject.Find("Enemy_Spawn").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("Enemy2_Spawn").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("Turret_Spawn").GetComponentsInChildren<Transform>();

                StartCoroutine(this.CreateEnemy(4));
                StartCoroutine(this.CreateEnemy2(4));
                StartCoroutine(this.CreateTurret());

                break;
            case STAGE.STAGE_06:

                //points = GameObject.Find ("Enemy_Spawn1").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("Enemy2_Spawn1").GetComponentsInChildren<Transform>();
                //points3 = GameObject.Find("Turret_Spawn1").GetComponentsInChildren<Transform>();

                //StartCoroutine(this.CreateEnemy(5));
                StartCoroutine(this.CreateEnemy2(5));
               // StartCoroutine(this.CreateTurret());

                break;
            case STAGE.STAGE_07:

                points = GameObject.Find("Enemy_Spawn2").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("Enemy2_Spawn2").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("Turret_Spawn2").GetComponentsInChildren<Transform>();

                StartCoroutine(this.CreateEnemy(6));
                StartCoroutine(this.CreateEnemy2(6));
                StartCoroutine(this.CreateTurret());

                break;
            case STAGE.STAGE_08:

                points = GameObject.Find("Enemy_Spawn3").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("Enemy2_Spawn3").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("Turret_Spawn3").GetComponentsInChildren<Transform>();

                StartCoroutine(this.CreateEnemy(7));
                StartCoroutine(this.CreateEnemy2(7));
                StartCoroutine(this.CreateTurret());

                break;
            case STAGE.Fin_Stage:

                points = GameObject.Find("Enemy_Spawn4").GetComponentsInChildren<Transform>();
                points2 = GameObject.Find("Enemy2_Spawn4").GetComponentsInChildren<Transform>();
                points3 = GameObject.Find("Turret_Spawn4").GetComponentsInChildren<Transform>();

                StartCoroutine(this.CreateEnemy(8));
                StartCoroutine(this.CreateEnemy2(8));
                StartCoroutine(this.CreateTurret());

                break;
            case STAGE.NONE_STAGE:

                break;
            default:

                break;
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            currentGold += 2000;
        }
    }

    private void MapChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("Dark City");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene("City inside");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SceneManager.LoadScene("CharacterScene");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SceneManager.LoadScene("EnemyScene");
        }

        //게임오버
        if(isGameOver)
        {
            StartCoroutine(this.GameOver());
        }
    }

    IEnumerator CreateEnemy(int stage)
    {

        //현재 생성된 적 케릭터 숫자 산출 
        int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

        
        //Debug.Log(enemyCount.ToString());
        //적 캐릭터의 최대 생성 개수보다 작을 때만 적 캐릭터를 생성
        if (enemyCount < maxEnemy)
        {
            //불규칙적인 위치 산출
            int idx = Random.Range(1, points.Length);
            //적 동적으로 생성
            GameObject stageMonster = Instantiate(enemy, points[idx].position, points[idx].rotation) as GameObject;
            if (stage == 0)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox1");
            else if (stage == 1)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox2");
            else if (stage == 2)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox3");
            else if (stage == 4)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox5");
            else if (stage == 5)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox6");
            else if (stage == 6)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox7");
            else if (stage == 7)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox8");
            else if (stage == 8)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox9");
            //적 캐릭터의 생성 시간만큼 대기! 
            yield return new WaitForSeconds(3.0f);
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator CreateEnemy2(int stage)
    {

        //현재 생성된 적 케릭터 숫자 산출 
        int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;
        //적 캐릭터의 최대 생성 개수보다 작을 때만 적 캐릭터를 생성
        if (enemyCount < maxEnemy2)
        {
            //불규칙적인 위치 산출
            int idx = Random.Range(1, points2.Length);
            //적 동적으로 생성
            GameObject stageMonster = Instantiate(enemy2, points2[idx].position, points2[idx].rotation) as GameObject;
            if (stage == 0)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox1");
            else if (stage == 1)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox2");
            else if (stage == 2)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox3");
            else if (stage == 4)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox5");
            else if (stage == 5)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox6");
            else if (stage == 6)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox7");
            else if (stage == 7)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox8");
            else if (stage == 8)
                stageMonster.GetComponent<Enemymove>().wayPointBox = GameObject.Find("WaypointBox9");
            //적 캐릭터의 생성 시간만큼 대기! 
            yield return new WaitForSeconds(3.0f);
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator CreateTurret()
    {      
        //현재 생성된 적 케릭터 숫자 산출 
        int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;
        //적 캐릭터의 최대 생성 개수보다 작을 때만 적 캐릭터를 생성
        if (enemyCount < maxTurret)
        {
            //불규칙적인 위치 산출
            int idx = Random.Range(1, points3.Length);
            //적 동적으로 생성
            Instantiate(turret, points3[idx].position, points3[idx].rotation);

            //적 캐릭터의 생성 시간만큼 대기! 
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            yield return null;
        }
    }

    public void UpdateCurrentGold()
    {
        goldText.text = currentGold.ToString();
    }

    public void DestroyEnemy()
    {
        GameObject[] currMonster = GameObject.FindGameObjectsWithTag("ENEMY");

        foreach(GameObject monster in currMonster)
        {
            GameObject.Destroy(monster);
        }
    }

    public void MagneticEffectOn()
    {
        if(currentGold >= 250 && !magneticEffect)
        {
            magneticEffect = true;
            currentGold -= 250;
            //Debug.Log("자석효과 구입 !");
        }
    }

    public void UpgradeHealEffect()
    {
        if(healEffect < 3 && currentGold >= 50)
        {
            healEffect++;
            currentGold -= 50;

            Debug.Log(healEffect.ToString());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene("Dead");
    }
}

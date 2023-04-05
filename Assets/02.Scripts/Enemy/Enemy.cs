﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy에 관한곳 
public class Enemy : MonoBehaviour
{
    //사용하는 컴포넌트 선언 
    //private Rigidbody rigidbody;
    private Animator animator;
    //몬스터의 상태 정의 
    public enum CurrentState {  Idle, Trace, Attack, Die }
    //몬스터 처음 상태 
    public CurrentState EnemyState = CurrentState.Idle;
    //플레이어 위치 저장
    private Transform PlayerTr;
    //몬스터 위치 저장 
    private Transform EnemyTr;
    //[SerializeField] LayerMask layerMask = 0;
    //[SerializeField] float Range = 30.0f;
    //공격 사정 거리 
    public float AttackRange = 0.0f;
    //추적 사정 거리 
    //public float TraceRange = 20.0f;

    // 사망 여부를 판단
    public bool IsDie = false;

    //코루틴 사용할 때 지연시간을 사용할 변수 
    private WaitForSeconds cTime;
    //이동을 제어하는 EnemyMove 클래스를 저장할 변수 
    private Enemymove enemymove;
    //총알 발사를 제어하는 EnemyFire 클래스를 저장할 변수 
    private EnemyFire enemyFire;
    //에너미 체력써야함
    private EnemyDamage enemyDamage;
    //시야각
    private EnemyFOV enemyFOV;

    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("IsDie");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");


  
    //한번만 호출되는 함수 
    void Awake()
    {

        
        //주인공의 게임 오브젝트를 추출 
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        //주인공의 Transform 컴포넌트 추출 
        if(player != null)
        {
            PlayerTr = player.GetComponent<Transform>();
        }
        //적 캐릭터의 Transform 컴포넌트 추출
        EnemyTr = GetComponent<Transform>();
        //컴포넌트 추출쓰~
        animator = GetComponent<Animator>();
        //이동을 제어하는 enemyMove 클래스 추출 
        enemymove = GetComponent<Enemymove>();
        //총 발사 추출 
        enemyFire = GetComponent<EnemyFire>();
        // 체력 추출
        enemyDamage = GetComponent<EnemyDamage>();
        // 시야각 및 추적 반경을 제어하는 EnemyFov 클래스 추출 
        enemyFOV = GetComponent<EnemyFOV>();

        // 코루틴 실행 지연되는시간. 
        cTime = new WaitForSeconds(0.3f);
        // WalkSpeed / Offset 값 변경 
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.3f));
    }

    // 활성화가 될 때마다 호출되는 함수 (중요! 활성화 될 때!!!! 마다)
    void OnEnable()
    {
        //CheckState코루틴 함수 실행
        StartCoroutine(CheckState());
        //Action 코루틴 함수를 실행
        StartCoroutine(Action());
    }
    // 에너미의 상태를 검사하는 코루틴함수 .
    IEnumerator CheckState()
    {
        //오브젝트 풀에 생성 시 다른 스크립트의 초기화를 위해 대기 
        yield return new WaitForSeconds(1.0f);
        //적 사망하기 전까지 도는 무한 루트 생성
        while (!IsDie)
        {
            // 현재 상태가 사망이면 이 코루틴 함수를 종료
            if (EnemyState == CurrentState.Die) yield break;

            // 주인공과 적 캐릭터 간의 거리를 계산  
            // range라고 플레이어와 몬그터의 거리를 두고  비고할꺼임.
            float range = Vector3.Distance(PlayerTr.position, EnemyTr.position);

            //공격 사정거리에 들어온  경우 
            if(range <= AttackRange)
            {
                //주인공과의 거리에 장애물 여부를 판단 
                if (enemyFOV.isViewPlayer())
                    EnemyState = CurrentState.Attack;
                //else
                //    EnemyState = CurrentState.Trace;
            }
            // 추적 거리  들어 왔을 때 
            else if (enemyFOV.isTracePlayer())
            {
                EnemyState = CurrentState.Trace;
            }
            // 그 외 아무것도 아닐 때 아이들 상태로 
            else
            {
                EnemyState = CurrentState.Idle;
            }

            // 코루틴 함수는 0.3초 간격으로 확인을 하기 때문에 
            yield return cTime;

        }
    }

    IEnumerator Action()
    {
        //죽을 때 까지 무한 루프 스타트
        while (!IsDie)
        {
            yield return cTime;

            switch (EnemyState)
            {
                case CurrentState.Idle:
                    // 총알 발사 정지
                    enemyFire.isFire = false;
                    //순찰모드
                    enemymove.patrolling = true;
                    break;
                case CurrentState.Trace:
                    //총알 발사 정지
                    enemyFire.isFire = false;
                    //주인공의 위치를 넘겨 추적 모드로 변경
                    enemymove.traceTarget = PlayerTr.position;
                    break;
                case CurrentState.Attack:
                    //순찰 및 추적을 정지하고 공격
                    enemymove.Stop();
                    //총알 발사 시작
                    if(enemyFire.isFire == false)
                    {
                        enemyFire.isFire = true;
                    }

                    break;
                case CurrentState.Die:
                    //죽었을 때
                    IsDie = true;
                    enemyFire.isFire = false;
                    enemymove.Stop();
                    animator.SetTrigger(hashDie);
                    GetComponent<BoxCollider>().enabled = false;
                    Destroy(this.gameObject, 2.0f);
                    break;
            }
        }
    }


    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Speed 파라미터에 이동 속도를 전달 
        animator.SetFloat(hashSpeed, enemymove.speed);
        
    }
}

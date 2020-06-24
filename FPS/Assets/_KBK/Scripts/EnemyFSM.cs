using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 유한상태머신
public class EnemyFSM : MonoBehaviour
{
    // 몬스터 상태 enum문
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState state; //몬스터 상태변수

    /// <summary>
    /// 유용한기능 (/// 치면 나옴)
    /// </summary>

    

    #region "Idle 상태에 필요한 변수들"
    #endregion
    #region "Move 상태에 필요한 변수들"
    #endregion
    #region "Attack 상태에 필요한 변수들"
    #endregion
    #region "Return 상태에 필요한 변수들"
    #endregion
    #region "Damaged 상태에 필요한 변수들"
    #endregion
    #region "Die 상태에 필요한 변수들"
    #endregion

    /// 필요한 변수들
    /// 
    public float findRange = 10f;  //플레이어 찾는 범위
    public float moveRange = 25f;   //시작 지접에서 최대 이용가능한 범위
    public float attackRange = 2f; //공격 가능 범위
    Vector3 spawnPos;  //시작지점
    Transform player;           //플레이어 찾기 위해서
    //GameObject player;
    CharacterController cc;     //캐릭터 이동 위해 캐릭터 컨트롤러 필요
    float moveSpeed = 5f;
    Quaternion startRotation;

    //애니메이션 제어하기 위한 애니메이터컴포넌트
    Animator anim;

    /// 몬스터 일반 변수
    float currHp;
    float maxHp = 10f;
    float att = 5f;      //공격력
    float speed = 5f;   //스피드

    //공격 딜레이
    float attTime = 2f; //2초에 한 번 공격
    float timer = 0;    //타이머

    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        spawnPos = transform.position;
        startRotation = transform.rotation;
        //플레이어 트랜스폼
        player = GameObject.Find("Player").GetComponent<Transform>();
        cc = GetComponent<CharacterController>();

        //애니메이터 컴포넌트
        anim = GetComponentInChildren<Animator>();

        currHp = maxHp;
    }
    
    void Update()
    {
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }//end of void Update()
    }

    private void Idle()
    {
        //1. 플레이어와 일정 범위가 되면 이동 상태로 변경
        // - 플레이어 찾기 (GameObject.Find("Player"))
        // - 일정거리 20미터(거리비교 : Distance, Magnitude 사용)

        //매그니튜드로 구하는 법
        //Vector3 dir = (transform.position - player.position);
        //float distance = dir.magnitude; //normalize 시켜주면 안됨 (1이 됨)
        //if(distance < findRange)
        if(Vector3.Distance(transform.position, player.transform.position) < findRange)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change State Idle to Move State");

            //애니메이션 
            anim.SetTrigger("Move");
        }
    }

    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격 범위 안에 들어오면 공격 모션 실행
        //2. 플레이어 추적하더라도 처음 위치에서 일정 범위 넘어가면 리턴 상태로 변경
        // - 플레이어처럼 캐릭터 컨트롤러를 이동하기

        // - 공격 범위 2미터
        if (Vector3.Distance(spawnPos, transform.position) > moveRange)
        {
            // - 상태 변경
            state = EnemyState.Return;
            anim.SetTrigger("Return");
            // - 상태 전환 출력
            print("Change State Move to Return State");
        }
        //moveRange를 벗어나지 않고 공격범위에 있지도 않음
        else if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            //플레이어 추격
            //이동방법 (벡터의 뺄셈)
            Vector3 dir = (player.transform.position - transform.position).normalized;
            //normalized된 dir -> 순수하게 방향으로만 씀

            //타겟을 바라보게 하자 
            //1. 방법1
            //transform.forward = dir;
            //2. 방법2
            //transform.LookAt(player);

            //좀 더 자연스럽게 회전처리 하고 싶다.
            //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
            // 위는 짐벌락 현상이 생김..둘이 같은 선상에 있으면 어떤 방향으로 회전할 지 몰라서 이상하게 회전함
            // 이유 : 회전처리를 벡터와 러프를 사용해서 해서 Quaternion으로 처리를 하지 않았기 때문에.

            // 최종적으로 자연스럽게 회전처리를 하려면 결국 쿼터니온 사용해야 한다.
            //forward 사용 안한다 (vector값이므로) / dir는 vector값이므로 Quaternion의 LookRotation을 사용한다.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            //Quaternion q = Quaternion.LookRotation(dir);
            //transform.rotation = q;
            //캐릭터 컨트롤러 이용해서 이동하기
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //중력값 있어야함(안그러면 떠버림)
            //중력문제를 해결하기 위해서 심플무브를 사용한다
            //SimpleMove : 최소한의 물리가 적용되어 중력문제를 해결 할 수 있다.
            //단 내부적으로 시간처리를 하기 때문에 Time.deltaTime을 사용하지 않는다
            cc.SimpleMove(dir * moveSpeed);
        }
        else //공격범위 안에 들어옴
        {
            // - 상태 변경
            state = EnemyState.Attack;
            // - 상태 전환 출력
            print("Change State Move to Attack State");
            anim.SetTrigger("Attack");
        }
    }

    private void Attack()
    {
        //1. 플레이어가 공격 범위 안에 있다면 일정한 시간 간격으로 플레이어 공격 (Timer)
        //2. 플레이어가 공격 범위 벗어나면 이동상태(재추격)
        // - 플레이어처럼 캐릭터 컨트롤러를 이동하기 
        Vector3 dir = (player.transform.position - transform.position).normalized;
        transform.forward = dir;
        // - 공격 범위 1미터
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if(timer > attTime)
            {
                //플레이어 피격처리 (플레이어 스크립트 안에서 가져오기)
                //player.GetComponent<PlayerMove>().hitDamage(att); //공격력을 받는 함수
                print("공격");
                anim.SetTrigger("Attack");
                //타이머 초기화
                timer = 0f;
            }
        }
        else //현재 상태를 Move로 변경(재추격)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change State Attack to Move State");
            timer = 0f;
            anim.SetTrigger("Move");
        }
    }

    //복귀 상태
    private void Return()
    {
        //시작 위치까지 도달하지 않을 때는 이동
        //도착하면 대기상태로 변경 
        // - 처음 위치에서 일정 범위 30미터 
        if (Vector3.Distance(transform.position, spawnPos) > 0.1f)
        {
            //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위 벗어나면 다시 돌아옴
            Vector3 dir = (spawnPos - transform.position).normalized;
            //transform.forward = dir;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            //심플무브는 내부적으로 시간처리함
            cc.SimpleMove(dir * moveSpeed);
        }
        else
        {
            //위치값을 초기값으로
            transform.position = spawnPos;
            // Idle로 돌아갈 때 회전값이 그대로 남아있는 현상 수정
            transform.rotation = Quaternion.identity;   //로테이션 초기화 
            // - 상태 변경
            state = EnemyState.Idle;
            // - 상태 전환 출력
            print("Change State Return to Idle State");

            anim.SetTrigger("Idle");
        }
        
    }

    //플레이어쪽에서 충돌감지를 할 수 있으니 이 함수는 퍼블릭으로 만들자
    public void hitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 혹은 상태일때는 데미지 중첩으로 주지 않는다
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //체력깍기
        currHp -= value;

        //몬스터의 체력이 1이상이면 피격상태
        if(currHp > 0)
        {
            state = EnemyState.Damaged;
            print("상태 : EnemyState -> Damaged");
            print("HP : " + currHp);
            anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            state = EnemyState.Die;
            print("상태 : EnemyState -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }

    //피격상태 (Any State)
    private void Damaged()
    {
        //코루틴 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전 상태로 변경
        // - 상태 변경
        // - 상태 전환 출력

        StartCoroutine(DamageProc());
    }

    private void Die()
    {
        //코루틴 사용하자
        //1. 체력이 0 이하
        //2. 몬스터 오브젝트 삭제
        // - 상태 변경
        // - 상태 전환 출력

        //혹시 진행중인 모든 코루틴을 정지한다
        StopAllCoroutines();

        StartCoroutine(DieProc());

    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1f);
        //상태 전환하기
        state = EnemyState.Move;
        print("상태 전환 : Damaged -> Move");
    }

    IEnumerator DieProc()
    {
        //굳이 안해줘도 되지만 오브젝트 날리기 전에 비활성화 해주는게 좋다.
        cc.enabled = false;

        //2초 후에 자기자신 제거
        yield return new WaitForSeconds(2f);
        print("죽음");
        Destroy(gameObject);
    }

    //시각적으로 범위 표시
    private void OnDrawGizmos()
    {
        //공격 가능 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //플레이어 탐지 범위 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);

        //이동 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPos, moveRange);
    }
}

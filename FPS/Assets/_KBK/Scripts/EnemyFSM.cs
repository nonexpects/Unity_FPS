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

    GameObject player;
    CharacterController cc;
    float moveSpeed = 5f;
    public Transform spawnPos;

    float currHp;
    float maxHp = 10f;

    #region "Idle 상태에 필요한 변수들"
    float findDist = 10f;
    #endregion
    #region "Move 상태에 필요한 변수들"
    float attDist = 4f;
    #endregion
    #region "Attack 상태에 필요한 변수들"
    #endregion
    #region "Return 상태에 필요한 변수들"
    float returnRange = 15f;
    #endregion
    #region "Damaged 상태에 필요한 변수들"
    #endregion
    #region "Die 상태에 필요한 변수들"
    #endregion

    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        player = GameObject.Find("Player");
        cc = GetComponent<CharacterController>();

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
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist < findDist)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change to Move State");
        }
    }

    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격 범위 안에 들어오면 공격 모션 실행
        //2. 플레이어 추적하더라도 처음 위치에서 일정 범위 넘어가면 리턴 상태로 변경
        // - 플레이어처럼 캐릭터 컨트롤러를 이동하기 
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = q; 
        cc.Move(dir * moveSpeed * Time.deltaTime);
        // - 공격 범위 2미터
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < attDist)
        {
            // - 상태 변경
            state = EnemyState.Attack;
            // - 상태 전환 출력
            print("Change to Attack State");
        }

        if (Vector3.Distance(spawnPos.transform.position, transform.position) >= returnRange)
        {
            // - 상태 변경
            state = EnemyState.Return;
            // - 상태 전환 출력
            print("Change to Return State");
        }
    }

    private void Attack()
    {
        //1. 플레이어가 공격 범위 안에 있다면 일정한 시간 간격으로 플레이어 공격 (Timer)
        //2. 플레이어가 공격 범위 벗어나면 이동상태(재추격)
        // - 플레이어처럼 캐릭터 컨트롤러를 이동하기 
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(dir);
        transform.rotation = q;
        float dist = Vector3.Distance(player.transform.position, transform.position);
        // - 공격 범위 1미터
        if (dist > attDist)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change to Move State");
        }
    }

    //복귀 상태
    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위 벗어나면 다시 돌아옴
        Vector3 dir = (spawnPos.position - transform.position).normalized;
        transform.LookAt(player.transform);
        cc.Move(dir * moveSpeed * 0.5f * Time.deltaTime);

        float dist = Vector3.Distance(player.transform.position, transform.position);
        // - 처음 위치에서 일정 범위 30미터 

        if (dist < findDist)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change to Move State");
        }
        if (dir.x < 0.1f)
        {
            // - 상태 변경
            state = EnemyState.Idle;
            // - 상태 전환 출력
            print("Change to Idle State");
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
    }

    private void Die()
    {
        //코루틴 사용하자
        //1. 체력이 0 이하
        //2. 몬스터 오브젝트 삭제
        // - 상태 변경
        // - 상태 전환 출력
    }

    IEnumerator Damaging()
    {
        while (currHp > 0)
        {


        }
    }


    IEnumerator Damaging()
    {

    }
}

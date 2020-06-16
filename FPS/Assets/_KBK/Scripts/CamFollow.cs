using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;    //카메라가 따라다닐 타겟
    public Transform target1;
    //플레이어한테 바로 카메라를 붙여서 이동해도 상관 없다
    //하지만 게임에 따라서 드라마틱한 연출이 필요한 경우에
    //타겟을 따라다니도록 하는게 1인칭에서 3인칭으로 또는 그 반대로 변경이 쉽다
    //또한 순간이동이 아닌 슈팅게임에서 꼬랑지가 따라다니는것 같은 효과도 만들 수 있다.
    //지금은 우리 눈 역할을 할거라서 그냥 순간이동 시킨다

    public float followSpeed = 10f;

    Vector3 camOffset = new Vector3(0f, 3f, -8f);
    Vector3 rotOffset = new Vector3(30f, 0f, 0f);

    

    bool thirdOn = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //카메라 위치를 강제로 타겟 위치에 고정해준다
        //transform.position = target.position;

        //FollowTarget();

        //1인칭 -> 3인칭, 3인칭 -> 1인칭으로 카메라 변경
        ChangeView();

        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    thirdOn = false;
        //}
        //else if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    thirdOn = true;
        //}


    }

    private void ChangeView()
    {
        if(Input.GetKeyDown("1"))
        {
            //transform.eulerAngles = rotOffset;
            thirdOn = true;
        }
        if (Input.GetKeyDown("3"))
        {
            //transform.eulerAngles = Vector3.zero;
            thirdOn = false;
        }

        if(thirdOn)
        {
            
            transform.position = target1.position;
        }
        else
        {
            
            transform.position = target.position;
        }
    }

    //타겟을 따라다니기
    private void FollowTarget()
    {
        //if (!thirdOn)
        //{
        //    //타겟 방향 구하기(벡터 뺄셈으로 처리)
        //    // 방향 = 타겟 - 자기자신
        //    Vector3 dir = target.position - transform.position;
        //    dir.Normalize();
        //
        //    transform.eulerAngles = Vector3.zero;
        //
        //    transform.Translate(dir * followSpeed * Time.deltaTime);
        //
        //    //문제점 : 타겟에 도착하면 덜덜덜 거림
        //    if (Vector3.Distance(target.position, transform.position) < 1f)
        //    {
        //        transform.position = target.position;
        //    }
        //}
        //else
        //{
        //    //타겟 방향 구하기(벡터 뺄셈으로 처리)
        //    // 방향 = 타겟 - 자기자신
        //    Vector3 dir = target.position - transform.position;
        //    dir.Normalize();
        //
        //    transform.eulerAngles = rotOffset;
        //
        //    transform.position = target.position + camOffset;
        //    
        //}
    }
}

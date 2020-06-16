using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //카메라를 마우스 움직이는 방향으로 회전하기
    public float speed = 150;   //각도라서 크게줌(1초에 150도)

    //회전 처리 직접 제어
    float angleX, angleY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateCam();
    }

    private void RotateCam()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        //회전처리는 축기반
        //마우스를 내리면 값이 증가하기 때문에 v는 반대로 처리해야 한다.
        //Vector3 dir = new Vector3(-v, h, 0);

        //transform.Rotate(dir * speed * Time.deltaTime);

        //유니티 엔진에서 제공해주는 함수를 사용함에 있어서
        //Translate함수는 그래도 사요 ㅇ하는데 큰 불편함이 없는데
        //회전을 담당하는 Rotate함수를 사용하면 제어하기가 힘들다
        //짐벌락 현상떄문에 => 인스펙터 창에 로테이션 값은 오일러지만 내부적으론 쿼터니온으로 사용하기 떄문
        //회전을 직접 제어 할 때는 Rotate 함수는 사용하지 않고 트랜스폼의 EulerAngle을 사용하면 된다.
        //현재 각도 + 방향 * 시간 = 미래 각도
        //P = P0 + vt 
        //transform.eulerAngles += dir * speed * Time.deltaTime;
        //카메라 문제 (-90 ~ 90) 고정했다 풀렸다 하는 문제가 발생함
        //직접 회전각도를 제한해서 처리하면 된다.
        //Vector3 angle = transform.eulerAngles;
        //angle += dir * speed * Time.deltaTime;
        //if (angle.x > 60) angle.x = 60;
        //if (angle.x < -60) angle.x = -60;
        //transform.eulerAngles = angle;

        //문제가 또 있음
        // 유니티 내부적으론 - 각도는 + 360를 해버림 그러니까 -60도면 300도가 됨
        // 내가 만든 각도를 가지고 계산처리해야 한다.

        angleX += h * speed * Time.deltaTime;
        angleY += v * speed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, -60f, 60f);
        angleY = Mathf.Clamp(angleY, -60f, 60f);
        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
    }
}

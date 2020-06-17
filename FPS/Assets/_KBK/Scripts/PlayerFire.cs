using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject fireFx;
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform firePoint;

    Rigidbody bombRg;
    Ray ray;
    RaycastHit hit;
    float distance = 15f;
    float throwPower = 8f;
    Vector3 throwAngle;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * distance, Color.red);
        Fire();
    }

    private void Fire()
    {
        //마우스 좌측 버튼 클릭시 레이캐스트로 총알 발사
        if(Input.GetMouseButtonDown(0))
        {
            //if(Physics.Raycast(transform.position, transform.forward, out hit, distance))
            //{
            //    //GameObject bullet = Instantiate(bulletPrefab);
            //    //bullet.transform.position = transform.position;
            //
            //    Quaternion rot = Quaternion.FromToRotation(-transform.forward, hit.normal);
            //    GameObject fx = Instantiate(fireFx, hit.point + (-hit.normal * 0.05f), rot);
            //    Destroy(fx, 1f);
            //}

            // 카메라 방향으로 설정 많이 함
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);   //origin, direction 정함
            //레이랑 충돌했냐?
            if(Physics.Raycast(ray, out hit))
            {
                //print("충돌오브젝트 : " + hit.collider.name);

                //호출 시점에 총알 이펙트 생성
                GameObject fx = Instantiate(fireFx);
                //부딪힌 지점의 정보는 hit안에 있다
                fx.transform.position = hit.point;
                //파편 이벤트
                //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다.
                fx.transform.forward = hit.normal;  //법선벡터로
                //이펙트가 위로 튀겨서 잘 안보였음
            }

            //레이어 마스크 사용 충돌체크(최적화)
            //유니티 내부적으로 속도 향상을 위해 비트연산 처니라 된다
            //총 32비트를 사용하기 때문에 레이어도 32개까지 추가가능함
            int layer = gameObject.layer;
            layer = 1 << 8; //1을 왼쪽으로 8번 이동해라
            //0000 0000 0000 0001 ( << 8)
            //0000 0001 0000 0000 => 2^7
            layer = 1 << 8 | 1 << 9 | 1 << 12;  // | : OR연산 3중 하나라도 참이면 참
            //0000 0001 0000 0000   => Player
            //0000 0010 0000 0000   => Enemy
            //0001 0000 0000 0000   => Boss
            //=> 0001 0011 0000 0000    => P, E, B 모두 충돌처리시킴
            if(Physics.Raycast(ray, out hit, 100f, layer))  //layer와 충돌처리
                //~layer면 저 레이어만 제외하고 충돌처리함
            {
                //비트 연산은 최적화에 도움이 되고 if문으로 번거롭게 하나하나 처리 안해도 됨
            }

        }
        //마우스 우측 버큰 클릭시 수류탄 투척하기
        //수류탄에 AddForce적용하면 중력 적용 받음 (rigidBody 필수)
        //AddForce는 던지는 사람(플레이어)쪽에서 처리해야됨
        //수류탄 - 자기 자신과 자기와 충돌한 오브젝트 파괴하는 역할만 함
        if (Input.GetMouseButtonDown(1))
        {
            //GameObject bomb = Instantiate(bombPrefab);
            //bomb.transform.position = transform.position;
            //throwAngle = transform.forward * 25f;
            //throwAngle.y = 8f;
            //Rigidbody rg = bomb.GetComponent<Rigidbody>();
            //rg.AddForce(throwAngle, ForceMode.Impulse);

            GameObject bomb = Instantiate(bombPrefab);
            //폭탄 플레이어가 던지기 떄문에 폭탄의 리지드 바디를 이욯해서 던지면 된다
            bomb.transform.position = firePoint.position;
            //전방으로 물리적인 힘을 가한다
            Rigidbody rg = bomb.GetComponent<Rigidbody>();
            //rg.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
            // ForceMode //
            //ForceMode.Acceleration => 연속적인 힘을 가함(질량 X)
            //ForceMode.Force => 연속적인 힘을 가한다(빌량의 영향 받음)
            //ForceMode.Impure -> 순간적인 힘을 가함(질량의 영향 받음)
            //ForceMode.VelocityCahge => 순간적인 힘을 가함(질량 X)

            //45도 각도로 발사
            //각도를 주려면 벡터의 덧셈 이용
            Vector3 dir = Camera.main.transform.forward + (Camera.main.transform.up*2f);     // 45도방향
            dir.Normalize();
            rg.AddForce(dir * throwPower, ForceMode.Impulse);
        }
    }
}

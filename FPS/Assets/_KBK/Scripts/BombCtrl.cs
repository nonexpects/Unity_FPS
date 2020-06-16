using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCtrl : MonoBehaviour
{
    //폭탄의 역할
    //예전 총알은 생성하면 지 스스로 날아가다 충돌하면 터졌다
    //하지만 폭탄은 생성되자 마자 스스로 이동하면 안되고 플레이어가 직접 던져야함
    //폭탄이 다른 오브젝트들과 충돌하면 터져야 한다
    //OnCollisionEnter로 충돌 확인(본인이 rigidbody가 있으므로)
    public GameObject explosionFx;
    float explodeCount;
    float explodeTime = 3f;
    Rigidbody rg;

    private void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //explodeCount += Time.deltaTime;
        //if (explodeCount > explodeTime)
        //{
        //    
        //}
    }

    private void Explosion()
    {
        

    }

    //충돌처리
    private void OnCollisionEnter(Collision collision)
    {
        // 1. 폭발 이펙트 보여주기
        GameObject fx = Instantiate(explosionFx);
        fx.transform.position = transform.position;
        Destroy(fx, 1.5f);  //1.5초 후에 삭제
                            // 2. 다른 오브젝트도 삭제하기
                            //Destroy(collision.collider);
                            // 3. 자기자신 삭제하기
        rg.AddExplosionForce(10f, transform.position, 10f);
        Destroy(gameObject);    //맨 마지막에 처리
    }
}

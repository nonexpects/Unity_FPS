using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    //Vector3 movement;
    CharacterController cotroller;

    void Start()
    {
        cotroller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //대각선 이동처리
        Vector3 movement = new Vector3(h, 0, v);
        //movement.Set(h, 0, v);
        //movement = movement.normalized * speed * Time.deltaTime;
        //transform.position += movement;
        
        //movement.Normalize();
        //transform.Translate(movement * speed * Time.deltaTime);

        //카메라가 보는 방향으로 이동처리
        movement = Camera.main.transform.TransformDirection(movement);
        //transform.Translate(movement * speed * Time.deltaTime);

        //문제점 : 하늘 날라다니고 땅 뚫고 충돌처리 안됨
        //rigidBody 붙이면 물리 적용받아서 문제 해결할 수 있지만 안씀 << 제어가 안됨
        //캐릭터 컨트롤러 컴포넌트 사용
        // 캐릭터 컨트롤러는 충돌 감지만 하고 물리가 적용 안됨
        //따라서 충돌 감지를 하기 위해서 반드시 캐릭터 컨트롤러 컴포넌트가 제공해주는 함수로 이동처리 해야한다.
        cotroller.Move(movement * speed * Time.deltaTime);
    }
}

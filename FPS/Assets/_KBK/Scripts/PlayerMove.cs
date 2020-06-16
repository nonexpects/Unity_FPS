using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    //Vector3 movement;
    CharacterController controller;
    public float gravity = -20f;    //아래방향(으로 당긴다)
    float velocityY;                //낙하속도(velocity는 방향과 힘을 들고 있다)
    float jumpPower = 10f;
    int jumpCount;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //대각선 이동처리 
        Vector3 movement = new Vector3(h, 0, v);
        //공중에 뜨는건 y값을 처리해줘야됨. (중력 필요)
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
        //controller.Move(movement * speed * Time.deltaTime);

        //중력 적용하기
        velocityY += gravity * Time.deltaTime;       //초당 -20씩 누적이 된다. (-20, -40, -60 ...)
        movement.y = velocityY;
        controller.Move(movement * speed * Time.deltaTime);

        //캐릭터의 점프
        //점프 버튼을 누르면 수직 속도에 점프 파워를 넣는다 (수직으로 올라가서 중력 누적돼서 내려감)
        //땅에 닿으면 0으로 초기화

        //if(controller.collisionFlags == CollisionFlags.Below)
        //coliisionFlag에는 Above, Below, Sides가 있음
        //character controller에 보이는 상 / 중 / 하부의 충돌을 감지

        if (controller.collisionFlags == CollisionFlags.Below)   //땅에 닿았냐?
        {
            velocityY = 0f;
            jumpCount = 0;
        }
        else
        {
            //땅에 닿지 않은 상태이기 때문에 중력 적용
            //velocityY += gravity * Time.deltaTime;       //초당 -20씩 누적이 된다. (-20, -40, -60 ...)
            //movement.y = velocityY;
        }
        
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            velocityY = jumpPower;  //velocityY에 10(jumpPower값)이 들어감 (올라감)
            jumpCount++;
        }
        //중력적용 이동
        //controller.Move(movement * speed * Time.deltaTime);
    }
}

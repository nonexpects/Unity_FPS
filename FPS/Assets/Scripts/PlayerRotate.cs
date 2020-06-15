using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    float speed = 150f;
    float angleX;

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        float h = Input.GetAxis("Mouse X");
        angleX += h * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, angleX, 0);
    }
}

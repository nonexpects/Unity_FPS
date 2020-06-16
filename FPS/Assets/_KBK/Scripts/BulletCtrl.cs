using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public GameObject fireFx;
    float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {         
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * 3f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //GameObject fx = Instantiate(fireFx);
        //fireFx.transform.position = transform.position;
        //Destroy(fx, 1f);
        //Destroy(this);
    }
}

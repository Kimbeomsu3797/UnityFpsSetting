using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float h;
    float z;
    float playerSpeed = 5f;
    
    CharacterController cc;
    float gravity = -20f;
    float yVelocity = 0;
    public float jumpPower = 10f;
    public bool isJumping = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, z);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        transform.position += dir * playerSpeed * Time.deltaTime;
        
        //transform.Translate(new Vector3(z*playerSpeed*Time.deltaTime, 0, h * playerSpeed * Time.deltaTime));
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }
        if (Input.GetButtonDown("Jump")&& !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * playerSpeed * Time.deltaTime);
    }
}

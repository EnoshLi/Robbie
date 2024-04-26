using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("基本组件")]
    private Rigidbody2D rigidbody2D;

    private BoxCollider2D coll;
    
    [Header("移动参数")]
    public  float normalSpeed;
    private float crounchSpeed;
    private float standSpeed;
    private float xVelocity;
    [Header("跳跃参数")] 
    public float jumpForce=6.3f;
    public float jumpHoldForce=1.9f;
    public float jumpHoldDuration=0.1f;
    [Header("碰撞体参数")]
    private Vector2 standUpOffset;
    private Vector2 standUpsize;
    private Vector2 crouchOffset;
    private Vector2 crouchSize;
    [Header("状态")]
    public bool isCrounch;
    public bool isJump;
    public bool isGround;
    //按键设置
    private bool jumpPressed;
    private bool jumpHeld;
    private bool crouchHeld;
    [Header("环境检测")]
    public LayerMask groundLayer;
    

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        standSpeed = 4f;
        crounchSpeed = 2f;
        //下蹲时改变人物碰撞体体型
        standUpOffset = coll.offset;
        standUpsize = coll.size;
        crouchOffset = new Vector2(coll.offset.x,coll.offset.y/2);
        crouchSize = new Vector2(coll.size.x, coll.size.y / 2);
    }

    private void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
    }

    private void FixedUpdate()
    {
        GrounchMove();
    }
    //人物移动
    private void GrounchMove()
    {
        if (crouchHeld)
        {
            Crouch();
        }
        else if (!Input.GetButton("Crouch") && isCrounch)
        {
            StandUp();
        }
        xVelocity = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new(xVelocity*normalSpeed, rigidbody2D.velocity.y);
        FlipPlayer();
    }
    //人物翻转
    private void FlipPlayer()
    {
        if (xVelocity<0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Crouch()
    {
        isCrounch = true;
        coll.size = crouchSize;
        coll.offset = crouchOffset;
        normalSpeed = crounchSpeed;
    }

    private void StandUp()
    {
        isCrounch = false;
        coll.size = standUpsize;
        coll.offset = standUpOffset;
        normalSpeed = standSpeed;
    }


}

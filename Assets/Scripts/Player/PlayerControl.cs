using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("基本组件")]
    private Rigidbody2D rd;

    private BoxCollider2D coll;
    
    [Header("移动参数")]
    public  float normalSpeed;
    private float crounchSpeed;
    private float standSpeed;
    private float xVelocity;
    [Header("跳跃参数")] 
    public float jumpForce;
    public float jumpHoldForce;
    public float jumpHoldDuration;
    public float jumpTime;
    public float crouchJumpBoost;
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
    public bool jumpPressed;
    //public bool jumpHeld;
    public bool crouchHeld;
    [Header("环境检测")]
    public LayerMask groundLayer;
    

    private void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        standSpeed = 4f;
        crounchSpeed = 2f;
        normalSpeed = standSpeed;
        //下蹲时改变人物碰撞体体型
        standUpOffset = coll.offset;
        standUpsize = coll.size;
        crouchOffset = new Vector2(coll.offset.x,coll.offset.y/2);
        crouchSize = new Vector2(coll.size.x, coll.size.y / 2);
    }

    private void Update()
    {
        jumpPressed = Input.GetButton("Jump");
        //jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GrounchMove();
        Jump();
        
    }
    //人物移动
    private void GrounchMove()
    {
        if (crouchHeld && !isCrounch && isGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrounch)
        {
            StandUp();
        }
        else if(isCrounch && !isGround)
        {
            StandUp();
        }
        xVelocity = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new(xVelocity*normalSpeed, GetComponent<Rigidbody2D>().velocity.y);
        FlipPlayer();
    }
    //人物翻转
    private void FlipPlayer()
    {
        //float faceDir = xVelocity;
        if (xVelocity<0)
        {
            this.transform.localScale = new Vector2(-1, 1);
        }
        else if(xVelocity>0)
        {
            this.transform.localScale = new Vector2(1, 1);
        }
    }
    
    //人物跳跃
    void Jump()
    {
        if (isGround && !isJump)
        {
            if (jumpPressed)
            {
                if (isCrounch && isGround)
                {
                    rd.AddForce(transform.up*crouchJumpBoost,ForceMode2D.Impulse);
                }
                isGround = false;
                isJump = true;
                jumpTime = Time.time + jumpHoldDuration;
                rd.AddForce(transform.up*jumpForce,ForceMode2D.Impulse);
            }
            
            
        } 
        if (isJump)
        {
            rd.AddForce(transform.up*jumpHoldForce,ForceMode2D.Impulse);
            if (jumpTime<Time.time)
            {
                isJump = false;
            }
        }
        
    }

    //人物下蹲碰撞体尺寸
    private void Crouch()
    {
        isCrounch = true;
        coll.size = crouchSize;
        coll.offset = crouchOffset;
        normalSpeed = crounchSpeed;
    }
    
    //人物站立碰撞体尺寸
    private void StandUp()
    {
        isCrounch = false;
        coll.size = standUpsize;
        coll.offset = standUpOffset;
        normalSpeed = standSpeed;
    }

    //人物地面检测
    void PhysicsCheck()
    {
        if (coll.IsTouchingLayers(groundLayer))
        {
            isGround = true;
            //isJump=false;
        }
        else
        {
            isGround = false;
        }
    }


}

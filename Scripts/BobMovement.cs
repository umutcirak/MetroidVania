using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float bounceSpeed;
    [SerializeField] float groundBreakerSpeed;
    [SerializeField] Vector2 deathPunch = new Vector2(0f, 10f);

    Vector2 moveInput;
    Rigidbody2D rb2dBob;
    Animator animatorBob;
    CapsuleCollider2D bodyColliderBob;
    BoxCollider2D feetColliderBob;

    bool BobHasHorizontalSpeed;    
    float defaultPlayerGravity;    

    bool isAlive = true;
    
    bool isJumping = false;
    bool isGroundBreaking = false;

    bool groundBreakerTriggered = false;

    int countSpaceBar;

    void Start()
    {
        moveSpeed = 5f;
        jumpSpeed = 15f;
        bounceSpeed = 1.25f;
        defaultPlayerGravity = 3f;
        groundBreakerSpeed = 15f;        

        countSpaceBar = 0;

        rb2dBob = GetComponent<Rigidbody2D>();
        animatorBob = GetComponent<Animator>();
        bodyColliderBob = GetComponent<CapsuleCollider2D>();
        feetColliderBob = GetComponent<BoxCollider2D>();

        rb2dBob.gravityScale = defaultPlayerGravity;

    }

    void Update()
    {
        if (!isAlive) { return; }   // disable player controls if he dies.

        Run();
        FlipSprite();        

    }
    void Run()
    {
        Vector2 bobVelocity = new Vector2(moveInput.x * moveSpeed, rb2dBob.velocity.y);
        // whatever velocity on y keep it. we don't want to touch y , 
        rb2dBob.velocity = bobVelocity;

        BobHasHorizontalSpeed = Mathf.Abs(rb2dBob.velocity.x) > Mathf.Epsilon;

        if(!isGroundBreaking && !isJumping)
        {
            animatorBob.SetBool("isRunning", BobHasHorizontalSpeed);
        }
        


    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();       
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (value.isPressed)
        {           

            if (countSpaceBar < 2)
            {
                isJumping = true;
                animatorBob.SetBool("isJumping", true);
                rb2dBob.velocity += new Vector2(0f, jumpSpeed);
                countSpaceBar++;
            }
            else if(countSpaceBar == 2)
            {
                countSpaceBar = 4; // disable Space Bar
                animatorBob.SetBool("isJumping", false);
                isJumping = false;
                GroundBreaker();               
            }
                       
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (feetColliderBob.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {            
            isGroundBreaking = false;            
            StartCoroutine(DisableGroundBreaker());
        }
    }
   
    IEnumerator DisableGroundBreaker()
    {
        yield return new WaitForSecondsRealtime(0.85f);
        countSpaceBar = 0;
        animatorBob.SetBool("isGroundBreaker", false);        

    }


    void GroundBreaker()
    {        
        Vector2 bobVelocity = new Vector2(rb2dBob.velocity.x, -groundBreakerSpeed);
        rb2dBob.velocity = bobVelocity;
        animatorBob.SetBool("isGroundBreaker", true);
        isGroundBreaking = true;
    }


    void FlipSprite()
    {
        BobHasHorizontalSpeed = Mathf.Abs(rb2dBob.velocity.x) > Mathf.Epsilon;
        float scale = Mathf.Sign(rb2dBob.velocity.x);

        if (BobHasHorizontalSpeed)
        {
            transform.localScale = new Vector3(scale, 1f, 1f);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AliceMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float bounceSpeed;
    [SerializeField] float forwardDashSpeed;
    [SerializeField] Vector2 deathPunch = new Vector2(0f, 10f);

    Vector2 moveInput;
    Rigidbody2D rigidBodyAlice;
    Animator animatorPlayer;
    CapsuleCollider2D bodyColliderAlice;
    BoxCollider2D feetColliderAlice;

    bool aliceHasHorizontalSpeed;
    bool playerHasVerticalSpeed;
    float defaultPlayerGravity;
    float forwardDashPeriod;

    bool isAlive = true;
    bool forwardDashTriggered = false;

    int countSpaceBar;


    void Start()
    {
        moveSpeed = 5f;
        jumpSpeed = 15f;
        bounceSpeed = 1.25f;
        defaultPlayerGravity = 3f;
        forwardDashSpeed = 30f;
        forwardDashPeriod = 0.75f;

        countSpaceBar = 0;

        rigidBodyAlice = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        bodyColliderAlice = GetComponent<CapsuleCollider2D>();
        feetColliderAlice = GetComponent<BoxCollider2D>();
        
        rigidBodyAlice.gravityScale = defaultPlayerGravity;



    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }   // disable player controls if he dies.
        Run();
        FlipSprite();
        Bounce();
        ForwardDashMovement();

    }
    


    void OnMove(InputValue value)
    {
        if (!isAlive && !forwardDashTriggered) { return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }
    
    
    void OnJump(InputValue value)
    {        
        if (!isAlive) { return; }     
                
        if (value.isPressed)
        {              

            if (countSpaceBar == 0)
            {
                rigidBodyAlice.velocity += new Vector2(0f, jumpSpeed);
                countSpaceBar = 1;
            }
            else if(countSpaceBar == 1)
            {
                forwardDashTriggered = true;
                countSpaceBar = 2; // disable jump and forwarddash until movement finish.
                StartCoroutine(DisableForwardDash());
            }
            
        }              
        
    }
  

    void ForwardDashMovement()
    {
        if (forwardDashTriggered)
        {
            float scale = GetComponent<AliceMovement>().transform.localScale.x;

            Vector2 aliceVelocity = new Vector2(forwardDashSpeed*scale, 0f);
            rigidBodyAlice.velocity = aliceVelocity;
            rigidBodyAlice.gravityScale = 0f;

        }
       
    }

    IEnumerator DisableForwardDash()
    {
        yield return new WaitForSecondsRealtime(forwardDashPeriod);
        forwardDashTriggered = false;
        rigidBodyAlice.gravityScale = defaultPlayerGravity;
        countSpaceBar = 0;

    }

    void Bounce()
    {
        if (!rigidBodyAlice.IsTouchingLayers(LayerMask.GetMask("Bouncing"))) { return; }

        Vector2 bounceVelocity = new Vector2(rigidBodyAlice.velocity.x, rigidBodyAlice.velocity.y + bounceSpeed);
        rigidBodyAlice.velocity = bounceVelocity;

    }
   
    
    void Run()
    {
        Vector2 aliceVelocity = new Vector2(moveInput.x * moveSpeed, rigidBodyAlice.velocity.y);
        // whatever velocity on y keep it. we don't want to touch y , 
        rigidBodyAlice.velocity = aliceVelocity;

        aliceHasHorizontalSpeed = Mathf.Abs(rigidBodyAlice.velocity.x) > Mathf.Epsilon;
        animatorPlayer.SetBool("isRunning", aliceHasHorizontalSpeed);

    }

    void FlipSprite()
    {         
        aliceHasHorizontalSpeed = Mathf.Abs(rigidBodyAlice.velocity.x) > Mathf.Epsilon;        
        float scale = Mathf.Sign(rigidBodyAlice.velocity.x);

        if (aliceHasHorizontalSpeed)
        {
            transform.localScale = new Vector3(scale, 1f, 1f);
        }
    }



    



}

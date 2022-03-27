using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AliceMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] Vector2 deathPunch = new Vector2(0f, 10f);

    Vector2 moveInput;
    Rigidbody2D rigidBodyAlice;
    Animator animatorPlayer;
    CapsuleCollider2D bodyColliderAlice;
    BoxCollider2D feetColliderAlice;

    bool aliceHasHorizontalSpeed;
    bool playerHasVerticalSpeed;
    float defaultPlayerGravity = 5f;

    bool isAlive = true;

    void Start()
    {
        moveSpeed = 5f;
        jumpSpeed = 15f;
        climbSpeed = 10f;

        rigidBodyAlice = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        bodyColliderAlice = GetComponent<CapsuleCollider2D>();
        feetColliderAlice = GetComponent<BoxCollider2D>();

        

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }   // disable player controls if he dies.
        Run();
        FlipSprite();

    }


    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    
    
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!feetColliderAlice.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        // press space bar no need a value
        if (value.isPressed)
        {
            rigidBodyAlice.velocity += new Vector2(0f, jumpSpeed);
        }
    }
    

    
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidBodyAlice.velocity.y);
        // whatever velocity on y keep it. we don't want to touch y , 
        rigidBodyAlice.velocity = playerVelocity;

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

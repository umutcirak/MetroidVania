using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobMovement : Player
{
    [SerializeField] float groundBreakerSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    bool isGroundBreaking = false;
    bool groundBreakerTriggered = false;
    bool isJumping = false;


    void Start()
    {
        base.Start();
        groundBreakerSpeed = 15f;

    }

    void Update()
    {
        base.Update();
    }
    public override void Run()
    {
        Vector2 bobVelocity = new Vector2(moveInput.x * moveSpeed, rigidBodyPlayer.velocity.y);
        // whatever velocity on y keep it. we don't want to touch y , 
        rigidBodyPlayer.velocity = bobVelocity;

        playerHasHorizontalSpeed = Mathf.Abs(rigidBodyPlayer.velocity.x) > Mathf.Epsilon;

        if (!isGroundBreaking && !isJumping)
        {
            animatorPlayer.SetBool("isRunning", playerHasHorizontalSpeed);
        }
    }

    public override void AbilityMovement()
    {
        Vector2 bobVelocity = new Vector2(rigidBodyPlayer.velocity.x, -groundBreakerSpeed);
        rigidBodyPlayer.velocity = bobVelocity;
        animatorPlayer.SetBool("isGroundBreaker", true);
        isGroundBreaking = true;
    }


    public override void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        Instantiate(bullet, gun.position, transform.rotation);
    }

    public override void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (value.isPressed)
        {

            if (countSpaceBar < 2)
            {
                isJumping = true;
                animatorPlayer.SetBool("isJumping", true);
                animatorPlayer.SetBool("isRunning", false);
                rigidBodyPlayer.velocity += new Vector2(0f, jumpSpeed);
                countSpaceBar++;
            }
            else if (countSpaceBar == 2)
            {
                countSpaceBar = 4; // disable Space Bar
                animatorPlayer.SetBool("isJumping", false);
                isJumping = false;
                AbilityMovement();
            }

        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (feetColliderPlayer.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGroundBreaking = false;
            isJumping = false;
            countSpaceBar = 0;
            animatorPlayer.SetBool("isJumping", false);
            StartCoroutine(DisableGroundBreaker());
        }
    }

    IEnumerator DisableGroundBreaker()
    {
        yield return new WaitForSecondsRealtime(0.85f);        
        animatorPlayer.SetBool("isGroundBreaker", false);

    }







}

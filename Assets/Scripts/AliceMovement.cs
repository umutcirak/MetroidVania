using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AliceMovement : Player
{
    [SerializeField] float fastSpeed;
    [SerializeField] float forwardDashPeriod;
    [SerializeField] float forwardDashSpeed;

    public bool isNotAttacking;
    bool forwardDashTriggered = false;


    void Start()
    {
        base.Start();
        float fastSpeed = 12f;
        moveSpeed = fastSpeed;
        forwardDashSpeed = 15f;
        forwardDashPeriod = 0.75f;
        isNotAttacking = true;        

    }

    void Update()
    {
        base.Update();
        AbilityMovement();

    }

    public override void Run()
    {
        Vector2 aliceVelocity = new Vector2(moveInput.x * moveSpeed, rigidBodyPlayer.velocity.y);
        // whatever velocity on y keep it. we don't want to touch y , 
        rigidBodyPlayer.velocity = aliceVelocity;

        playerHasHorizontalSpeed = Mathf.Abs(rigidBodyPlayer.velocity.x) > Mathf.Epsilon;

        if (isNotAttacking && !forwardDashTriggered)
        {
            animatorPlayer.SetBool("isRunning", playerHasHorizontalSpeed);
        }
    }

    public override void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (value.isPressed)
        {

            if (countSpaceBar == 0)
            {
                rigidBodyPlayer.velocity += new Vector2(0f, jumpSpeed);
                countSpaceBar = 1;
            }
            else if (countSpaceBar == 1)
            {
                forwardDashTriggered = true;
                countSpaceBar = 2; // disable jump and forwarddash until movement finish.
                StartCoroutine(DisableForwardDash());
            }
        }

    }
    public override void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        if (isNotAttacking)
        {
            isNotAttacking = false;

            animatorPlayer.SetBool("isAttacking", true);
            StartCoroutine(DisableAttacking());
        }
    }

    public override void AbilityMovement()
    {
        if (forwardDashTriggered)
        {
            float scale = GetComponent<AliceMovement>().transform.localScale.x;

            Vector2 aliceVelocity = new Vector2(forwardDashSpeed * scale, 0f);
            rigidBodyPlayer.velocity = aliceVelocity;
            rigidBodyPlayer.gravityScale = 0f;
            animatorPlayer.SetBool("isForwardDash", forwardDashTriggered);

        }

    }    

    IEnumerator DisableAttacking()
    {
        yield return new WaitForSecondsRealtime(0.833f);
        animatorPlayer.SetBool("isAttacking", false);
        isNotAttacking = true;
    }

    IEnumerator DisableForwardDash()
    {
        yield return new WaitForSecondsRealtime(forwardDashPeriod);
        forwardDashTriggered = false;
        rigidBodyPlayer.gravityScale = defaultPlayerGravity;
        countSpaceBar = 0;
        animatorPlayer.SetBool("isForwardDash", forwardDashTriggered);

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (feetColliderPlayer.IsTouchingLayers(LayerMask.GetMask("Ground")) && !forwardDashTriggered)
        {
            countSpaceBar = 0;
        }
    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour, IInteractable
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float bounceAccelaration;
    [SerializeField] public Vector2 deathSpeed;
    [SerializeField] public float defaultPlayerGravity;

    public Vector2 moveInput;
    public Rigidbody2D rigidBodyPlayer;
    public Animator animatorPlayer;
    public CapsuleCollider2D bodyColliderPlayer;
    public BoxCollider2D feetColliderPlayer;

    public bool playerHasHorizontalSpeed;

    public bool isAlive;
    public int countSpaceBar;


    public void Start()
    {
        moveSpeed = 5f;
        jumpSpeed = 17f;
        bounceAccelaration = 1f;
        defaultPlayerGravity = 3f;
        deathSpeed = new Vector2(0f, 10f);
        isAlive = true;
        countSpaceBar = 0;

        rigidBodyPlayer = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        bodyColliderPlayer = GetComponent<CapsuleCollider2D>();
        feetColliderPlayer = GetComponent<BoxCollider2D>();

        rigidBodyPlayer.gravityScale = defaultPlayerGravity;


    }

    public void Update()
    {
        if (!isAlive) { return; }   // disable player controls if he dies.
        Run();
        FlipSprite();
        Bounce();
        Die();
    }

    public void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        //Debug.Log(moveInput);
    }

    public abstract void OnJump(InputValue value);
    public abstract void AbilityMovement();
    public void Bounce()
    {
        if (!rigidBodyPlayer.IsTouchingLayers(LayerMask.GetMask("Bouncing"))) { return; }


        float fallingVelocity = Mathf.Abs(rigidBodyPlayer.velocity.y);
        Vector2 bounceVelocity = new Vector2(rigidBodyPlayer.velocity.x, fallingVelocity + bounceAccelaration);
        rigidBodyPlayer.velocity = bounceVelocity;
    }
    public abstract void OnFire(InputValue value);

    public abstract void Run();

    public void FlipSprite()
    {
        playerHasHorizontalSpeed = Mathf.Abs(rigidBodyPlayer.velocity.x) > Mathf.Epsilon;
        float scale = Mathf.Sign(rigidBodyPlayer.velocity.x);

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector3(scale, 1f, 1f);
        }
    }
    public void Die()
    {
        if (bodyColliderPlayer.IsTouchingLayers(LayerMask.GetMask("Enemies", "Danger")) ||
            feetColliderPlayer.IsTouchingLayers(LayerMask.GetMask("Enemies", "Danger")))
        {
            isAlive = false;
            animatorPlayer.SetTrigger("Dying");
            rigidBodyPlayer.velocity = deathSpeed;
            // if dies reload the level
            StartCoroutine(ProcessDie());
            
            

        }
    }

    IEnumerator ProcessDie()
    {
        yield return new WaitForSecondsRealtime(1.75f);
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float enemySpeed = 1f;

    Rigidbody2D enemyRigidBody;
    CapsuleCollider2D enemyBodyCollider;

    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        enemyRigidBody.velocity = new Vector2(enemySpeed, 0f);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        enemySpeed = -enemySpeed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }



}

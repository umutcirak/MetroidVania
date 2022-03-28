using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb2dBullet;
    BobMovement bob;
    float xSpeed;
    [SerializeField] float bulletSpeed;
    void Start()

    {
        bulletSpeed = 10f;
        rb2dBullet = GetComponent<Rigidbody2D>();

        bob = FindObjectOfType<BobMovement>();

        xSpeed = bob.transform.localScale.x * bulletSpeed;

    }
    
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        rb2dBullet.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}

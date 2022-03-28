using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStroke : MonoBehaviour
{
    Rigidbody2D rb2dStroke;
    AliceMovement alice;

    void Start()
    {
        rb2dStroke = GetComponent<Rigidbody2D>();
        alice = FindObjectOfType<AliceMovement>();

    }
    
    void Update()
    {       
        Fire();
    }

    void Fire()
    {
        if (alice.isNotAttacking)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }
        rb2dStroke.velocity = new Vector2(0f, 0f);       
        
        GetComponent<BoxCollider2D>().enabled = true;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }       

    }
     

}

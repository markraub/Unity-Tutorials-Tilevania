using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D enemyrb;
    Collider2D wallCollider;


    [Header("Physics")]
    [SerializeField] float moveSpeed = 1f;
    

    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyrb = GetComponent<Rigidbody2D>();
        wallCollider = GetComponent<BoxCollider2D>();
        
    }

    void Update()
    {
        
        Walk(moveSpeed);
        bool hasHorizontalVelocity = Mathf.Abs(enemyrb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", hasHorizontalVelocity); 
       

    }

    void Walk(float direction)
    {
        enemyrb.velocity = new Vector2(direction, 0);

    }

    void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
    }

}

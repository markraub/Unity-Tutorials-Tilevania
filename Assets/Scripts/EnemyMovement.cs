using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D enemyrb;
    
    


    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyrb = GetComponent<Rigidbody2D>();
        
        
        
    }

    void Update()
    {
        //walk(random direction between -1 and 1)
        
    }

    void Walk(float direction)
    {

    }
}

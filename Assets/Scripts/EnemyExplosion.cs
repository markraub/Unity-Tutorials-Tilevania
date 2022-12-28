using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    Collider2D circleCollider;
    Animator anim;
    AudioSource audioSource;
    
    void Awake() 
    {
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (circleCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            circleCollider.enabled = false;
            anim.SetTrigger("Zap");
            audioSource.Play();

        }
    }

    
}

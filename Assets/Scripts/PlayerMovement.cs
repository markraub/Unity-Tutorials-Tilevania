using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    AudioSource playerAudioSource;

    SpriteRenderer playerSpriteRenderer;

    Collider2D playerCollider;
    Collider2D feetCollider;

    float startingGravity;

    bool isAlive;


    [Header("Player Physics")]
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float climbSpeed;
    [SerializeField] float deathFling;

    [Header("Sound Effects")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip climbSound;

    void Awake() 
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerAudioSource = GetComponent<AudioSource>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        startingGravity = playerRigidBody.gravityScale;

        isAlive = true;

    }


    void Update()
    {
        bool hasVerticalVelocity = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;

        if (!isAlive && !hasVerticalVelocity)
        {
            playerAnimator.SetTrigger("Dead");
        }

        if (!isAlive) { return;}
        Run();
        FlipSprite();
        Climb();
        Die();
        

    }

    void OnMove(InputValue val)
    {
        if (!isAlive) { return;}
        moveInput = val.Get<Vector2>();

    }

    void Run()
    {

        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;
        

        
    }

    void FlipSprite()
    {
        bool playerMoving = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1);
            playerAnimator.SetBool("isRunning", true);
            
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }


    }

    void Climb()
    {

        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            playerRigidBody.gravityScale = startingGravity;
            playerAnimator.SetBool("isClimbing", false);
            return; 
        }
        playerRigidBody.gravityScale = 0f;            
        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
        playerRigidBody.velocity = climbVelocity;
        
        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;

        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);

    
    }


    void OnJump(InputValue val)
    {
        if (!isAlive) { return;}
        if(val.isPressed && feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {

            playerRigidBody.velocity += new Vector2(0, jumpHeight);
            playerAudioSource.PlayOneShot(jumpSound, 0.5f);
        }
        
    }

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerSpriteRenderer.color = Color.red;
            playerRigidBody.velocity = new Vector2(0, 0);
            playerRigidBody.velocity += new Vector2(0, deathFling);

        }
    }

}

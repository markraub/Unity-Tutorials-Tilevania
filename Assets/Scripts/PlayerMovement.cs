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

    Collider2D playerCollider;

    float startingGravity;


    [Header("Player Physics")]
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float climbSpeed;

    [Header("Sound Effects")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip climbSound;

    void Awake() 
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAudioSource = GetComponent<AudioSource>();
        startingGravity = playerRigidBody.gravityScale;

    }


    void Update()
    {
        Run();
        FlipSprite();
        Climb();
    }

    void OnMove(InputValue val)
    {
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
        if(val.isPressed && playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {

            playerRigidBody.velocity += new Vector2(0, jumpHeight);
            playerAudioSource.PlayOneShot(jumpSound, 0.5f);
        }
        
    }





}

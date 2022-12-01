using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;

    Collider2D playerCollider;


    [Header("Player Physics")]
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float climbSpeed;

    void Awake() 
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();

    }

    void Start()
    {
         
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

        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
            playerRigidBody.velocity = climbVelocity;

            if (playerRigidBody.velocity.y > 0)
            {
                playerAnimator.SetBool("isClimbing", true);
            }
            
        }
        else
        {
            playerAnimator.SetBool("isClimbing", false);
        }

    }


    void OnJump(InputValue val)
    {
        if(val.isPressed && playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            playerRigidBody.velocity += new Vector2(0, jumpHeight);
        }
        
    }



}

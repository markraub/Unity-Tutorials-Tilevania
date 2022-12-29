using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    AudioSource playerAudioSource;

    SpriteRenderer playerSpriteRenderer;

    ParticleSystem JumpParticles;

    Collider2D playerCollider;
    Collider2D feetCollider;

    float startingGravity;

    bool isAlive;




    [Header("Player Physics")]
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float climbSpeed;
    [SerializeField] float deathFling;
    [SerializeField] PhysicsMaterial2D alivePM;
    [SerializeField] PhysicsMaterial2D deadPM;

    [Header("Sound Effects")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip climbSound;
    [SerializeField] AudioClip footstepSound;

    [Header("Combat")]
    [SerializeField] GameObject Claw;
    [SerializeField] GameObject SwipeObject;
    [SerializeField] float SwipeCooldown = 0.25f;

    void Awake() 
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerAudioSource = GetComponent<AudioSource>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        JumpParticles = GetComponent<ParticleSystem>();
        startingGravity = playerRigidBody.gravityScale;
        playerCollider.sharedMaterial = alivePM;
        

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
        SwipeCooldown -= Time.deltaTime;
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
            JumpParticles.Play();
            playerRigidBody.velocity += new Vector2(0, jumpHeight);
            playerAudioSource.PlayOneShot(jumpSound, 0.5f);
        }
        
    }


    void OnFire(InputValue val) 
    {
        if (isAlive && SwipeCooldown <= 0)
        {
            GameObject SwipeInstance = Instantiate(SwipeObject, Claw.transform.position, Claw.transform.rotation); 
            SwipeCooldown = 0.25f;
        }
        
    }

    void Die()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRigidBody.velocity = new Vector2(0, 0);
            playerRigidBody.velocity += new Vector2(0, deathFling);
            playerCollider.sharedMaterial = deadPM;
            StartCoroutine("ReloadLevel");
            
        }
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 3)
        {
            JumpParticles.Stop();
            playerAudioSource.PlayOneShot(footstepSound, 0.5f);
        }

        
    }

}

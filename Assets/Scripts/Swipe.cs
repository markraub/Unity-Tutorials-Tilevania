using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{

    Rigidbody2D SwipeRB;

    PlayerMovement player;

    AudioSource SwipeSound;

    float xspeed;

    [SerializeField] float SwipeSpeed = 2f;
    [SerializeField] float Lifetime = 0.1f;

    [Header("SFX")]
    [SerializeField] AudioClip[] SlashSFX;
    [SerializeField] AudioClip[] HitSFX;

    private void Awake() 
    {
        SwipeRB = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        SwipeSound = GetComponent<AudioSource>();


    }
    void Start()
    {
        xspeed = player.transform.localScale.x * SwipeSpeed;
        gameObject.transform.localScale = new Vector3(player.transform.localScale.x, 1f, 1f);
        Destroy(gameObject, Lifetime);

        SwipeSound.clip = SlashSFX[Random.Range(0, SlashSFX.Length)];
        SwipeSound.Play();

    }

    // Update is called once per frame
    void Update()
    {
        SwipeRB.velocity = new Vector2 (xspeed, 0f);
                
    }

    private void OnCollisionEnter2D(Collision2D other) {
        SwipeSound.clip = HitSFX[Random.Range(0, HitSFX.Length)];
        SwipeSound.Play();

        if (other.gameObject.tag == "Enemy")
        {
            EnemyMovement Enemy = other.gameObject.GetComponent<EnemyMovement>();
            Enemy.Kill();
        }
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{

    BoxCollider2D doorCollider;

    AudioSource doorAudioSource;

    SpriteRenderer doorSpriteRenderer;

    bool CanExit = false;

    [SerializeField] AudioClip doorOpenSFX;
    [SerializeField] AudioClip doorKnockSFX;
    [SerializeField] Sprite doorOpenSprite;
    [SerializeField] float loaddelay;



    void Awake()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        doorAudioSource = GetComponent<AudioSource>();
        doorSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int enemies = FindObjectsOfType<EnemyMovement>().Length;

        if (enemies <= 0)
        {
            if (!CanExit)
            {
                doorAudioSource.PlayOneShot(doorOpenSFX);
                doorSpriteRenderer.sprite = doorOpenSprite;
                CanExit = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            if (CanExit)
            {
                StartCoroutine("LoadNextLevel");
            }
        }
        
    }

    IEnumerator LoadNextLevel()
    {
        int activescene = SceneManager.GetActiveScene().buildIndex;
        activescene += 1;
        yield return new WaitForSecondsRealtime(loaddelay);
        Debug.Log(activescene);
        SceneManager.LoadScene(activescene);
    }
}

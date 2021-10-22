using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenMan : MonoBehaviour
{
    public Transform head;
    public WeaponData_SO weaponData;
    public AudioSource playingAudio;
    public AudioSource movingAudio;

    private Rigidbody2D playerRigidbody2D;
    private bool isReady = false;
    private bool isHeadTurning = false;
    private float duration = 5f;
    private float interval = 10f;
    private Coroutine coroutine;
    void Start()
    {
        playerRigidbody2D = GameObject.FindWithTag("Player").gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isReady && playerRigidbody2D.velocity != Vector2.zero)
        {
            playerRigidbody2D.gameObject.GetComponent<PlayerStats>().TakeDamage(weaponData);
        }
        if (isReady)
        {
            if(!isHeadTurning) {
                head.transform.Rotate(0, 180, 1);
                isHeadTurning = true;
            }
        } else {
            if (isHeadTurning) {
                head.transform.Rotate(0, 180, 1);
                isHeadTurning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coroutine = StartCoroutine(Timer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(coroutine);
            isReady = false;

            movingAudio.Stop();
            playingAudio.Stop();

            if(KeyCollision.isFinish == true)
            {
                this.enabled = false;
                movingAudio.enabled = false;
                playingAudio.enabled = false;
            }
        }
    }

    private void InvokeSetReady()
    {
        isReady = true;
        movingAudio.Play();
        playingAudio.Stop();
    }

    IEnumerator Timer()
    {
        for (int i = 0; i < 30; i++)
        {
            isReady = false;
            playingAudio.Play();
            movingAudio.Stop();
            Invoke("InvokeSetReady", duration);
            yield return new WaitForSeconds(duration + interval + 1f);
        }
    }
}

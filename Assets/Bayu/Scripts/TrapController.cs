using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapController : MonoBehaviour
{

    public enum TrapType
    {
        None,
        Disappear,
        Appear,
        Fall,
        Blink,
        SpikePop,
        Fake
    }

    public TrapType trapType;
    public GameObject target;

    [Header("General Settings")]
    public float delay = .3f;

    [Header("Fall Settings")]
    public float disableColliderDelay = .1f;

    [Header("BlinkSettings")]
    public float blinkOnTime = 1f;
    public float blinkOffTime = 1f;

    [Header("SpikeSettings")]
    public GameObject spike;

    private Rigidbody2D rb;
    private Collider2D col;
    private bool triggered;
    private bool blinking;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) return;

        rb = target.GetComponent<Rigidbody2D>();
        col = target.GetComponent<Collider2D>();

        if (trapType == TrapType.Appear)
        {
            target.SetActive(false);
        }

        if (trapType == TrapType.Fall && rb == null)
        {
            rb = target.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (trapType == TrapType.Fake && col != null)
        {
            col.enabled = false;
        }

        if (spike != null) {
            spike.SetActive(false);

        }

        if (trapType == TrapType.Blink && !blinking)
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER ENTER: " + collision.name);

        if (!collision.CompareTag("Player")) return;

        switch (trapType)
        {
            case TrapType.Disappear:
                Disappear();
                break;

            case TrapType.Appear:
                target.SetActive(true);
                break;

            case TrapType.Fall:
                HandleFall();
                break;
            case TrapType.SpikePop:
                if (spike != null)
                {
                    Invoke(nameof(ActiveSpike), delay);
                }
                break;
            case TrapType.Blink:
                if (!blinking)
                {
                    StartCoroutine(BlinkRoutine());
                }
                break;

        }
    }

    void HandleFall()
    {
        if(rb == null || col == null) return;

        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(DisableColliderAfterDelay());

    }
    
    void Disappear()
    {
        target.SetActive(false);
    }

    void ActiveSpike()
    {
        spike.SetActive(true);
    }

    IEnumerator DisableColliderAfterDelay()
    {
        yield return new WaitForSeconds(disableColliderDelay);
        col.enabled = false;
    }

    IEnumerator BlinkRoutine()
    {
        blinking = true;
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        Collider2D col = target.GetComponent<Collider2D>();
        while (true) {

            sr.enabled = true;
            col.enabled = true;
            yield return new WaitForSeconds(blinkOnTime);

            sr.enabled = false;
            col.enabled = false;
            yield return new WaitForSeconds(blinkOffTime);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            if (transform.position.x >= rightPoint.position.x)
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

            if (transform.position.x <= leftPoint.position.x)
            {
                Flip();
            }
        }

        sr.flipX = !movingRight;
    }

    void Flip()
    {
        movingRight = !movingRight;
    }

    void OnDrawGizmos()
    {
        if (leftPoint != null && rightPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(leftPoint.position, rightPoint.position);
            Gizmos.DrawSphere(leftPoint.position, 0.1f);
            Gizmos.DrawSphere(rightPoint.position, 0.1f);
        }
    }
}

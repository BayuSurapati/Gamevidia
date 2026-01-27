using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    public float playerMoveSpeed;
    public float playerJumpForce;


    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundCheckRadius = .2f;
    [SerializeField]
    LayerMask groundMask;

    private Rigidbody2D rb;
    private bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        PlayerMove();
        PlayerJump();
    }

    void PlayerMove()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2 (moveInput * playerMoveSpeed, rb.velocity.y);
    }

    void PlayerJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    }
}

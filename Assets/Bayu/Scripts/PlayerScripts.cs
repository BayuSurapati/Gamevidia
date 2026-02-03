using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    [Header("Movement")]
    public float playerMoveSpeed;
    public float playerJumpForce;

    [Header("GroundCheck")]
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundCheckRadius = .2f;
    [SerializeField]
    LayerMask groundMask;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(.5f, .5f, .5f);
        }else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-.5f, .5f, .5f);
        }

            rb.velocity = new Vector2(moveInput * playerMoveSpeed, rb.velocity.y);
    }

    void PlayerJump()
    {
        anim.SetBool("IsGrounded", isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
            isGrounded = false;
            anim.SetBool("IsGrounded", !isGrounded);
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

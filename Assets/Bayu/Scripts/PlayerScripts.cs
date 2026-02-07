using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool canMove = true;

    //New Input System
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    // Start is called before the first frame update

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        ReadMovement();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += OnJump;
    }

    void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Disable();
    }

    void ReadMovement() //Membaca nilai vector 2 dari input actions yang sudah dibuat
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (!isGrounded) return;

        rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
        B_AudioManager.Instance.PlaySFX(9);
        isGrounded = false;
        anim.SetBool("IsGrounded", false);
    }

    //Ini merupakan gerakan player menggunakan input system lama

    /*void PlayerMove()
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
    }*/

    void MovePlayer()
    {
        if (!canMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        rb.velocity = new Vector2(moveInput.x * playerMoveSpeed, rb.velocity.y);

        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(.35f, .35f, .35f);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-.35f, .35f, .35f);
        }
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", Mathf.Abs(moveInput.x));
        anim.SetBool("IsGrounded", isGrounded);
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

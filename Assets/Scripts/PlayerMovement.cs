using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Lật nhân vật
        if (moveInput > 0)
            sr.flipX = false;
        else if (moveInput < 0)
            sr.flipX = true;

        // Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
     
        // Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            jumpRequested = true;

        // Nhặt đồ
        if (Input.GetKeyDown(KeyCode.F))
            Debug.Log("Nhặt đồ!");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }
}
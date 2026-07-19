using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Interaction Settings")]
    public Transform interactPoint;
    public float interactRadius = 0.8f;
    public LayerMask interactLayer;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // States
    private float moveInput;
    private bool isGrounded;
    private bool jumpRequested = false;

    // Hướng nhìn
    private Vector2 facingDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogWarning("Không tìm thấy Animator trên nhân vật!");
        }
    }

    void Update()
    {
        // 1. Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Lật nhân vật + cập nhật hướng nhìn
        if (moveInput > 0)
        {
            sr.flipX = false;
            facingDirection = Vector2.right;
        }
        else if (moveInput < 0)
        {
            sr.flipX = true;
            facingDirection = Vector2.left;
        }

        // 3. Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 4. Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }

        // 5. Animation
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            anim.SetBool("isJumping", !isGrounded);
        }

        // 6. Tương tác
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithItem();
        }
    }

    void FixedUpdate()
    {
        // Di chuyển
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Nhảy
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    // =========================
    // INTERACT SYSTEM (UPGRADE)
    // =========================
    private void InteractWithItem()
    {
        if (interactPoint == null)
        {
            Debug.LogWarning("Chưa gắn interactPoint!");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(interactPoint.position, interactRadius, interactLayer);

        if (hits.Length == 0)
        {
            Debug.Log("Không có gì trong vùng tương tác!");
            return;
        }

        float closestDistance = Mathf.Infinity;
        Interactable bestTarget = null;

        foreach (var hit in hits)
        {
            // DEBUG xem có detect được không
            Debug.Log("Detect: " + hit.name);

            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable == null) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                bestTarget = interactable;
            }
        }

        if (bestTarget != null)
        {
            Debug.Log("Tương tác với: " + bestTarget.name);
            bestTarget.Interact();
        }
        else
        {
            Debug.Log("Có object nhưng KHÔNG có Interactable!");
        }
    }

    // Vẽ gizmos
    private void OnDrawGizmosSelected()
    {
        if (interactPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(interactPoint.position, interactRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)facingDirection * 2f);
        }
    }
}
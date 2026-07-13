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
    public float interactRadius = 0.5f;
    public LayerMask interactLayer;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // States
    private float moveInput;
    private bool isGrounded;
    private bool jumpRequested = false;

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
        // 1. Nhận input di chuyển
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Lật nhân vật
        if (moveInput > 0)
            sr.flipX = false;
        else if (moveInput < 0)
            sr.flipX = true;

        // 3. Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
     
        // 4. Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }

        // 5. Cập nhật Hoạt ảnh (Animation)
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
            anim.SetBool("isJumping", !isGrounded);
        }

        // 6. Nhặt đồ / Tương tác
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithItem();
        }
    }

    void FixedUpdate()
    {
        // Xử lý di chuyển ngang
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Xử lý nhảy
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    // Hàm xử lý tương tác
    private void InteractWithItem()
    {
        if (interactPoint == null)
        {
            Debug.LogWarning("Chưa gắn interactPoint vào Player!");
            return;
        }

        // Quét một hình tròn tại vị trí interactPoint để tìm đồ vật thuộc interactLayer
        Collider2D hit = Physics2D.OverlapCircle(interactPoint.position, interactRadius, interactLayer);
        
        if (hit != null)
        {
            // Kiểm tra xem vật thể chạm vào có chứa script kế thừa IInteractable không
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
            {
                Debug.Log($"Phát hiện {hit.gameObject.name} nhưng nó không có script IInteractable.");
            }
        }
        else
        {
            Debug.Log("Không có gì để tương tác ở trước mặt.");
        }
    }

    // Hàm hỗ trợ vẽ vòng tròn tương tác trong màn hình Scene (để dễ setup)
    private void OnDrawGizmosSelected()
    {
        if (interactPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(interactPoint.position, interactRadius);
        }
    }
}
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

    [Header("Jump Quality Settings")]
    [Tooltip("Thời gian đệm sau khi rời đất để vẫn nhảy được")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    [Tooltip("Thời gian lưu lệnh nhảy trước khi chạm đất")]
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    // States
    private float moveInput;
    private bool isGrounded;
    private bool jumpRequested = false;
    private Vector3 initialScale;

    [Header("Quest Settings")]
    public bool hasWatch = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        if (anim == null)
        {
            Debug.LogWarning("Không tìm thấy Animator trên nhân vật!");
        }

        // Lưu lại kích thước ban đầu được thiết lập trong Unity Editor
        initialScale = transform.localScale;

        // Kiểm tra an toàn cho groundCheck để tránh NullReferenceException
        if (groundCheck == null)
        {
            Debug.LogWarning("Chưa gắn groundCheck! Đang tự động tìm kiếm đối tượng con tên GroundCheck...");
            groundCheck = transform.Find("GroundCheck");
            if (groundCheck == null)
            {
                Debug.LogError("Không tìm thấy GroundCheck! Vô hiệu hóa script di chuyển để tránh crash.");
                enabled = false;
                return;
            }
        }
    }

    void Update()
    {
        // 1. Nhận input di chuyển
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Lật nhân vật bằng localScale nhưng giữ nguyên kích thước ban đầu (initialScale) để lật các đối tượng con
        if (moveInput > 0)
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);

        // 3. Kiểm tra chạm đất
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
     
        // Xử lý Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // 4. Nhảy (Sử dụng Coyote Time & Jump Buffer)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            jumpRequested = true;
            jumpBufferCounter = 0f; // Reset bộ đệm nhảy
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
            coyoteTimeCounter = 0f; // Reset Coyote Time để tránh nhảy thêm một lần nữa trên không
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
            // Lấy TẤT CẢ các script kế thừa IInteractable trên vật thể
            IInteractable[] interactables = hit.GetComponents<IInteractable>();
            bool interacted = false;
            
            foreach (var interactable in interactables)
            {
                // Chỉ chạy Interact nếu script đó đang được Tích Xanh (Enabled)
                if (interactable is MonoBehaviour mb && mb.enabled)
                {
                    interactable.Interact();
                    interacted = true;
                    break;
                }
            }
            
            if (!interacted && interactables.Length == 0)
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
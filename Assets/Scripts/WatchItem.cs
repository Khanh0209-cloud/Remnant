using UnityEngine;

public class WatchItem : MonoBehaviour, IInteractable
{
    [Header("Quest Settings")]
    [Tooltip("Tên vật phẩm phục vụ nhiệm vụ")]
    public string itemName = "Đồng hồ của Mion";

    [Header("Audio Settings")]
    [Tooltip("Âm thanh phát ra khi nhặt vật phẩm")]
    public AudioClip pickupSFX;

    [Range(0f, 1f)]
    [Tooltip("Âm lượng hiệu ứng âm thanh (từ 0.0 đến 1.0)")]
    public float sfxVolume = 1f; // <-- Thêm dòng này để tạo thanh kéo âm lượng trong Inspector

    // =========================================================================
    // --- 1. THÊM MỚI: Khai báo biến cho hình ảnh gợi ý (Interact Prompt) ---
    // =========================================================================
    [Header("UI Prompt")]
    [Tooltip("Kéo đối tượng hình ảnh hiển thị nút bấm vào đây")]
    public GameObject interactPrompt;

    [Tooltip("Khoảng cách để hình ảnh hiện ra")]
    public float promptDistance = 2f;
    private Transform playerTransform; // Biến ẩn dùng để nhớ vị trí người chơi
    // =========================================================================
    // --- 2. THÊM MỚI: Hàm Start() để tìm nhân vật lúc mới vào game ---
    // =========================================================================
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }
    // =========================================================================
    // --- 3. THÊM MỚI: Hàm Update() để đo khoảng cách liên tục mỗi khung hình ---
    // =========================================================================
    void Update()
    {
        // Nếu bạn quên chưa gán hình ảnh hoặc game lỗi không tìm thấy người chơi thì bỏ qua
        if (interactPrompt == null || playerTransform == null) return;
        // Tính toán khoảng cách
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        // Nếu khoảng cách <= 2f thì bật hình ảnh, ngược lại thì tắt
        if (distance <= promptDistance)
        {
            interactPrompt.SetActive(true);
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }

    public void Interact()
    {
        NPC mionScript = Object.FindObjectOfType<NPC>();

        if (mionScript != null && mionScript.hasSpokenPhase1 == false)
        {
            // Vẫn CHO PHÉP nhặt đồng hồ, nhưng in ra gợi ý để debug
            Debug.Log("Gợi ý: Người chơi chưa nói chuyện với Mion lần đầu!");
        }

        // Tìm kiếm đối tượng Player trong Scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Cập nhật trạng thái đã nhặt đồng hồ của người chơi
                playerMovement.hasWatch = true;
                Debug.Log($"Đã nhặt thành công: {itemName}!");

                // Phát âm thanh tại vị trí hiện tại của chiếc đồng hồ trước khi hủy nó
                if (pickupSFX != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position, sfxVolume);
                }

                // Hủy vật phẩm khỏi màn chơi sau khi nhặt
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Player với Tag 'Player'!");
        }
    }
}

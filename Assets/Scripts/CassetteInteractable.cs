using UnityEngine;

public class CassetteInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo Panel chứa giao diện giải đố đài Cassette vào đây")]
    public GameObject puzzlePanel;

    [Tooltip("Kéo nhân vật Nox vào đây để khóa di chuyển khi đang giải đố")]
    public PlayerMovement player;

    void Start()
    {
        // Ẩn bảng giải đố lúc mới vào game
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
    }

    // Hàm này được gọi tự động khi Nox đứng gần đài Cassette và bấm phím F
    public void Interact()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true);
            
            // Khóa nhân vật không cho chạy đi chỗ khác
            if (player != null) player.enabled = false;
            
            Debug.Log("Đã mở bảng giải đố đài Cassette!");
        }
        else
        {
            Debug.LogWarning("Bạn chưa kéo Panel giải đố vào script CassetteInteractable!");
        }
    }

    // Gắn hàm này vào sự kiện OnClick() của nút "Thoát" hoặc lớp "Background" màng chắn
    public void ClosePuzzle()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
            
            // Thả tự do cho nhân vật chạy tiếp
            if (player != null) player.enabled = true;
            
            Debug.Log("Đã đóng bảng giải đố đài Cassette.");
        }
    }
}

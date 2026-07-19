using UnityEngine;
using UnityEngine.UI;

public class PaperReaderUI : MonoBehaviour
{
    public static PaperReaderUI Instance;
    
    [Tooltip("Kéo Panel chứa UI mảnh giấy vào đây")]
    public GameObject panel;
    
    [Tooltip("Kéo Image dùng để hiển thị nội dung mảnh giấy vào đây")]
    public Image paperImageDisplay;
    
    [Tooltip("Kéo nhân vật Nox vào đây để khóa di chuyển khi đang đọc giấy")]
    public PlayerMovement player;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        if (panel != null) panel.SetActive(false);
    }
    
    // Gọi hàm này để bung ảnh mảnh giấy lên màn hình
    public void ShowPaper(Sprite paperContentSprite)
    {
        if (panel != null && paperImageDisplay != null && paperContentSprite != null)
        {
            paperImageDisplay.sprite = paperContentSprite;
            panel.SetActive(true);
            
            // Phát âm thanh nhặt giấy
            if (AudioManager.Instance != null) AudioManager.Instance.PlayPickupPaper();
            
            // Khóa di chuyển khi đang xem giấy
            if (player != null) player.enabled = false;
        }
    }
    
    // Gọi hàm này khi click ra ngoài màn hình để đóng giấy
    public void ClosePaper()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            
            // Phát âm thanh thả giấy
            if (AudioManager.Instance != null) AudioManager.Instance.PlayPickupPaper();
            
            // Mở lại di chuyển
            if (player != null) player.enabled = true;
            
            // Kích hoạt sự kiện rung lắc và xuất hiện Cassette nếu đã đọc xong 3 tờ
            if (Chapter2Manager.Instance != null && Chapter2Manager.Instance.papersCollected >= 3)
            {
                Chapter2Manager.Instance.TriggerCassetteEvent();
            }
        }
    }
}

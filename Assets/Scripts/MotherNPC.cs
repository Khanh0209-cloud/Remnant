using UnityEngine;
using System.Collections;

public class MotherNPC : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo vật thể NPC Người Con (đang ẩn) vào đây")]
    public GameObject sonNPC;
    
    private bool isFinished = false;

    public void Interact()
    {
        // Tránh bấm nhiều lần khi đang chạy sự kiện
        if (isFinished) return;

        if (Chapter2Manager.Instance != null && Chapter2Manager.Instance.hasNecklace)
        {
            Debug.Log("Chole: Ôi... đây là sợi dây chuyền của con trai tôi...");
            isFinished = true;
            StartCoroutine(EndingSequence());
        }
        else
        {
            Debug.Log("Chole: Xin hãy giúp tôi tìm món đồ quan trọng ấy...");
        }
    }

    private IEnumerator EndingSequence()
    {
        // 1. Kích hoạt Con xuất hiện
        if (sonNPC != null) sonNPC.SetActive(true);
        Debug.Log("Đứa bé dần hiện ra... 'Mom...'");
        
        // 2. Chờ 3 giây cho cảm động
        yield return new WaitForSeconds(3f);
        
        // 3. Tan biến
        Debug.Log("Hai mẹ con ôm nhau và tan biến thành ánh sáng...");
        
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.isLevelPassed = true;
            
            // Phá vỡ Level Blocker
            if (Chapter2Manager.Instance.levelBlocker != null)
            {
                Destroy(Chapter2Manager.Instance.levelBlocker);
                Debug.Log("Màn chắn cửa đã vỡ vụn! Bạn có thể đi tiếp sang màn sau.");
            }
        }
        
        // Xóa Mẹ và Con khỏi màn chơi
        if (sonNPC != null) sonNPC.SetActive(false);
        gameObject.SetActive(false);
    }
}

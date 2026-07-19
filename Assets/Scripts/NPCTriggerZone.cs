using UnityEngine;

public class NPCTriggerZone : MonoBehaviour
{
    [Tooltip("Kéo NPC Mẹ (đang bị ẩn) vào đây")]
    public GameObject npcMother;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem người chạm vào vùng này có phải là Player không
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true;
            if (npcMother != null)
            {
                npcMother.SetActive(true);
                Debug.Log("Người mẹ đã đột ngột xuất hiện!");
            }
        }
    }
}

using UnityEngine;

public class NPCTriggerZone : MonoBehaviour
{
    [Header("NPC Configuration")]
    [Tooltip("Kéo GameObject của NPC Mion đang bị ẩn ở màn chơi vào đây")]
    public GameObject npcToActivate;

    [Tooltip("Tự động kích hoạt đối thoại ngay khi NPC xuất hiện")]
    public bool autoStartDialogue = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player không
        if (other.CompareTag("Player"))
        {
            if (npcToActivate != null)
            {
                // Kích hoạt NPC Mion
                npcToActivate.SetActive(true);
                Debug.Log($"{npcToActivate.name} đã được kích hoạt xuất hiện!");

                // Tự động khởi chạy hội thoại nếu được thiết lập
                if (autoStartDialogue)
                {
                    NPC npcScript = npcToActivate.GetComponent<NPC>();
                    if (npcScript != null)
                    {
                        npcScript.Interact();
                    }
                }
            }
            else
            {
                Debug.LogWarning("Chưa kéo thả NPC vào mục npcToActivate của Trigger Zone!");
            }

            // Tự hủy Trigger Zone này để nó chỉ kích hoạt duy nhất một lần
            Destroy(gameObject);
        }
    }
}

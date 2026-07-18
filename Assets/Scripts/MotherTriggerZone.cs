using UnityEngine;

public class MotherTriggerZone : MonoBehaviour
{
    [Tooltip("Kéo vật thể NPC Mẹ vào đây")]
    public MotherNPC motherNPC;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem người chạm vào có phải là Player (Nox) không
        if (other.CompareTag("Player"))
        {
            if (motherNPC != null)
            {
                motherNPC.Appear();
            }
            
            // Xóa vùng Trigger này đi để Mẹ không xuất hiện lại lần thứ hai
            Destroy(gameObject);
        }
    }
}

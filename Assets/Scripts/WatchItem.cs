using UnityEngine;

public class WatchItem : MonoBehaviour, IInteractable
{
    [Header("Quest Settings")]
    [Tooltip("Tên vật phẩm phục vụ nhiệm vụ")]
    public string itemName = "Đồng hồ của Mion";

    public void Interact()
    {
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

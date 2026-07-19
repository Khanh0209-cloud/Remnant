using UnityEngine;

public class SuitcaseInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo Panel_Suitcase vào đây")]
    public GameObject suitcaseUIPanel;
    
    [Tooltip("Kéo nhân vật Nox vào đây để tạm khóa di chuyển khi đang mở UI")]
    public PlayerMovement player;

    public void Interact()
    {
        if (suitcaseUIPanel != null)
        {
            suitcaseUIPanel.SetActive(true);
            
            // Phát âm thanh
            if (AudioManager.Instance != null) AudioManager.Instance.PlayOpenedVali();
            
            // Khóa nhân vật không cho chạy lung tung lúc đang mở UI
            if (player != null)
            {
                player.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("Chưa gắn Panel_Suitcase vào SuitcaseInteractable!");
        }
    }
}

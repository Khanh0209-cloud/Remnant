using UnityEngine;

public class SuitcaseOpenedInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo Panel_TeddyBearPuzzle (Mini-game) vào đây")]
    public GameObject teddyBearPanel;
    
    public PlayerMovement player;

    public void Interact()
    {
        if (teddyBearPanel != null)
        {
            teddyBearPanel.SetActive(true);
            if (player != null) player.enabled = false; // Khóa di chuyển
        }
    }
}

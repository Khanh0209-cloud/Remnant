using UnityEngine;

public class PaperInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // Khi tương tác (nhấn F), báo cho Manager biết đã nhặt được 1 mảnh giấy
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.AddPaper();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Chapter2Manager trong màn chơi!");
        }
        
        // Xóa vật thể mảnh giấy khỏi màn hình
        Destroy(gameObject);
    }
}

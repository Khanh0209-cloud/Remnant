using UnityEngine;

public class TeddyBearInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Nox: Có vết rách sau lưng gấu... Bạn đã tìm thấy Dây Chuyền!");
        
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.hasNecklace = true;
        }
        
        // Nhặt xong dây chuyền, con gấu biến mất
        Destroy(gameObject); 
    }
}

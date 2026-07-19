using UnityEngine;

public class Meat : MonoBehaviour
{
    // Đảm bảo hàm này nằm độc lập bên trong class, không nằm lồng trong Start hay Update
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player không
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false); // Ẩn cục thịt đi khi người chơi chạm vào
        }
    }
}
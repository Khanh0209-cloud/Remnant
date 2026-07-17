using UnityEngine;

public class Chapter2Manager : MonoBehaviour
{
    // Cung cấp một biến tĩnh để các script khác dễ dàng gọi tới Manager
    public static Chapter2Manager Instance;
    
    [Header("Trạng thái màn chơi")]
    public int papersCollected = 0;
    
    [Header("Đồ vật sự kiện")]
    [Tooltip("Kéo đài Cassette đang bị ẩn ở Hierarchy vào đây")]
    public GameObject cassettePlayer;

    void Awake()
    {
        // Đảm bảo chỉ có 1 Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPaper()
    {
        papersCollected++;
        Debug.Log("Đã nhặt " + papersCollected + "/3 mảnh giấy.");
        
        // Khi nhặt đủ 3 mảnh, làm xuất hiện đài Cassette
        if (papersCollected >= 3)
        {
            if (cassettePlayer != null)
            {
                cassettePlayer.SetActive(true);
                Debug.Log("Cassette Player đã bất ngờ xuất hiện trên ghế!");
            }
        }
    }
}

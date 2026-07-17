using UnityEngine;

public class Chapter2Manager : MonoBehaviour
{
    public static Chapter2Manager Instance;
    
    [Header("Trạng thái màn chơi")]
    public int papersCollected = 0;
    public bool hasTalkedToMother = false; // Mới thêm: Phải nói chuyện xong mới được nhặt giấy
    public bool hasNecklace = false;
    public bool isLevelPassed = false;
    
    [Header("Đồ vật sự kiện")]
    public GameObject cassettePlayer;
    public GameObject levelBlocker; // Tường chặn

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private bool hasTriggeredCassette = false;

    public void AddPaper()
    {
        papersCollected++;
        Debug.Log("Đã nhặt " + papersCollected + "/3 mảnh giấy.");
    }

    public void TriggerCassetteEvent()
    {
        if (hasTriggeredCassette) return;
        hasTriggeredCassette = true;
        
        if (cassettePlayer != null) cassettePlayer.SetActive(true);
        
        // Rung màn hình
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.TriggerShake(0.5f, 0.1f);
        }
    }
}

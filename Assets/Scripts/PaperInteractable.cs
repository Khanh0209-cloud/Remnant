using UnityEngine;

public class PaperInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo bức ảnh (Sprite) chứa Nội dung của mảnh giấy này vào đây")]
    public Sprite paperContentSprite;
    
    private SpriteRenderer sr;
    private Color originalColor;
    
    [Tooltip("Thứ tự bắt buộc của mảnh giấy này (1, 2, 3)")]
    public int paperIndex = 1;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    void Update()
    {
        // Hiệu ứng phát sáng nếu đã được phép nhặt VÀ tới đúng lượt nhặt mảnh này
        if (Chapter2Manager.Instance != null && Chapter2Manager.Instance.hasTalkedToMother)
        {
            // Chỉ sáng lên nếu tới lượt của nó
            if (Chapter2Manager.Instance.papersCollected + 1 == paperIndex)
            {
                if (sr != null)
                {
                    // Nhấp nháy màu vàng nhạt
                    float pingPong = Mathf.PingPong(Time.time * 2f, 1f);
                    sr.color = Color.Lerp(originalColor, new Color(1f, 1f, 0.5f, 1f), pingPong);
                }
            }
        }
    }

    public void Interact()
    {
        // Phải nói chuyện với Mẹ trước mới được nhặt giấy
        if (Chapter2Manager.Instance != null && !Chapter2Manager.Instance.hasTalkedToMother)
        {
            Debug.Log("Nox: Mình chưa nên đụng vào mấy thứ này lúc này...");
            return;
        }

        // Bắt buộc nhặt theo thứ tự
        if (Chapter2Manager.Instance != null && Chapter2Manager.Instance.papersCollected + 1 != paperIndex)
        {
            Debug.Log("Nox: Mảnh giấy này chắp vá không khớp, mình nên tìm mảnh số " + (Chapter2Manager.Instance.papersCollected + 1) + " trước.");
            return;
        }

        // Bật UI để đọc giấy
        if (PaperReaderUI.Instance != null)
        {
            PaperReaderUI.Instance.ShowPaper(paperContentSprite);
        }

        // Báo cho Manager biết đã nhặt được 1 mảnh giấy
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.AddPaper();
        }
        
        // Hủy mảnh giấy dưới đất
        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class DragToTearUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public TeddyBearPuzzleUI puzzleManager;
    
    [Tooltip("Khoảng cách cần kéo để mảnh lưng đứt ra")]
    public float tearThreshold = 100f;
    
    private Vector2 startPos;
    private RectTransform rectTransform;
    
    [HideInInspector]
    public bool isTorn = false;

    [Tooltip("Kéo nửa lưng còn lại vào đây để chúng kiểm tra chéo nhau")]
    public DragToTearUI otherHalf;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // Ghi nhớ vị trí ban đầu để nếu kéo trượt thì nảy về
        if (rectTransform != null) startPos = rectTransform.anchoredPosition;
        isTorn = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Không làm gì nếu đã bị xé
        if (isTorn) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isTorn) return;
        
        // Di chuyển ảnh theo chuột
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isTorn) return;
        
        // Tính khoảng cách đã kéo
        float distance = Vector2.Distance(startPos, rectTransform.anchoredPosition);
        
        if (distance > tearThreshold)
        {
            isTorn = true;
            Debug.Log(gameObject.name + " đã bị xé toạc!");
            
            // Nếu nửa kia cũng đã xé xong thì báo cho Manager bật Bông và Dây chuyền
            if (otherHalf != null && otherHalf.isTorn)
            {
                if (puzzleManager != null)
                {
                    puzzleManager.OnBearTornApart();
                }
            }
        }
        else
        {
            // Kéo chưa đủ xa thì lò xo nảy về chỗ cũ
            rectTransform.anchoredPosition = startPos;
        }
    }
}

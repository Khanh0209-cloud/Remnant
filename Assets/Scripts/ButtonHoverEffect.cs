using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    
    [Header("Opacity Settings")]
    [Tooltip("Độ mờ của chữ khi chuột bình thường (0.0 đến 1.0). 0.85 = 85%")]
    public float normalAlpha = 0.85f;
    
    [Tooltip("Độ sáng của chữ khi chuột trỏ vào (0.0 đến 1.0). 1.0 = 100%")]
    public float hoverAlpha = 1.0f;

    [Header("Hover Effect Object")]
    [Tooltip("Kéo thả GameObject chứa hình ảnh hiệu ứng trang trí vào đây. Nó sẽ được bật/tắt tự động.")]
    public GameObject hoverEffectObject;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    void Start()
    {
        // Khởi tạo màu chữ với độ mờ bình thường
        SetAlpha(normalAlpha);

        // Đảm bảo tắt hiệu ứng khi mới bắt đầu
        if (hoverEffectObject != null)
        {
            hoverEffectObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Khi chuột trỏ vào nút: Làm sáng chữ và bật hiệu ứng
        SetAlpha(hoverAlpha);
        
        if (hoverEffectObject != null)
        {
            hoverEffectObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Khi chuột rời khỏi nút: Làm mờ chữ và tắt hiệu ứng
        SetAlpha(normalAlpha);

        if (hoverEffectObject != null)
        {
            hoverEffectObject.SetActive(false);
        }
    }

    private void SetAlpha(float alphaValue)
    {
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alphaValue;
            buttonImage.color = color;
        }
    }
}

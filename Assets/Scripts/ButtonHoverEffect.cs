using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    
    [Header("Opacity Settings")]
    [Tooltip("Độ mờ khi chuột bình thường (0.0 đến 1.0). 0.85 = 85%")]
    public float normalAlpha = 0.85f;
    
    [Tooltip("Độ mờ khi chuột trỏ vào (0.0 đến 1.0). 1.0 = 100%")]
    public float hoverAlpha = 1.0f;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    void Start()
    {
        // Khởi tạo màu với độ mờ bình thường
        SetAlpha(normalAlpha);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Khi chuột trỏ vào nút
        SetAlpha(hoverAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Khi chuột rời khỏi nút
        SetAlpha(normalAlpha);
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

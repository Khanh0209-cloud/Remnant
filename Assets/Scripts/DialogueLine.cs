using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Header("Thông tin nhân vật")]
    public string speaker;   // tên người nói (Hazel, Nox, Sói...)
    public bool isLeft;      // true = tên hiển thị bên trái, false = tên hiển thị bên phải

    [Header("Nội dung lời thoại")]
    [TextArea(3, 5)]         // Giúp ô nhập text trong Inspector rộng rãi, dễ xuống dòng hơn
    public string text;
}
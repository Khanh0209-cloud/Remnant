using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingTextAnimation : MonoBehaviour
{
    private TMP_Text loadingText;
    public string baseText = "Loading screen";
    public float animationDelay = 0.4f;

    void Start()
    {
        // Lấy component TextMeshPro
        loadingText = GetComponent<TMP_Text>();
        if (loadingText != null)
        {
            StartCoroutine(AnimateText());
        }
    }

    private IEnumerator AnimateText()
    {
        int dotCount = 0;
        while (true)
        {
            // Tạo chuỗi dấu chấm dựa trên dotCount (từ 0 đến 3)
            string dots = new string('.', dotCount);
            loadingText.text = baseText + dots;

            // Tăng số chấm, nếu bằng 4 thì quay lại 0
            dotCount = (dotCount + 1) % 4;

            // Đợi trước khi chuyển sang frame tiếp theo
            yield return new WaitForSeconds(animationDelay);
        }
    }
}

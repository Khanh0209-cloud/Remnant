using UnityEngine;
using System.Collections;

public class KeyFragmentInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo nhân vật Nox vào đây để khóa di chuyển khi chìa khóa đang từ từ hiện ra")]
    public PlayerMovement player;
    
    [Tooltip("Thời gian để chìa khóa hiện rõ (giây)")]
    public float fadeInDuration = 2f;

    private SpriteRenderer sr;
    private bool isReadyToPickUp = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Đặt opacity (kênh Alpha) về 0 để làm chìa khóa hoàn toàn trong suốt lúc ban đầu
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }
        
        // Tắt vật thể đi để chờ Mẹ gọi ra
        gameObject.SetActive(false);
    }

    // Hàm này được gọi bởi Mẹ khi Mẹ biến mất
    public void StartFadeIn()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        // 1. Chặn di chuyển của Nox
        if (player != null) player.enabled = false;

        float elapsedTime = 0f;
        Color c = sr.color;

        // 2. Tăng dần opacity từ 0 lên 1 trong suốt 2 giây
        while (elapsedTime < fadeInDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            if (sr != null) sr.color = c;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo opacity đạt tròn 100%
        c.a = 1f;
        if (sr != null) sr.color = c;
        
        isReadyToPickUp = true;
        
        // 3. Mở khóa cho Nox di chuyển lại để tiến tới nhặt chìa khóa
        if (player != null) player.enabled = true;
        
        Debug.Log("Mảnh chìa khóa đã hiện hình rõ ràng, có thể nhặt!");
    }

    public void Interact()
    {
        // Nếu chưa hiện rõ thì không cho nhặt
        if (!isReadyToPickUp) return;

        // Báo hiệu đã qua màn và phá màn chắn
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.isLevelPassed = true;
            
            if (Chapter2Manager.Instance.levelBlocker != null)
            {
                Destroy(Chapter2Manager.Instance.levelBlocker);
                Debug.Log("Đã nhặt chìa khóa! Màn chắn cửa đã vỡ vụn!");
            }
        }

        // Phát âm thanh nhặt chìa khóa
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPickupKey();
            
            // Phát âm thanh Complete sau 1 giây
            AudioManager.Instance.PlayCompleteWithDelay(1f);
        }

        // Tiêu hủy mảnh chìa khóa khỏi mặt đất
        Destroy(gameObject);
    }
}

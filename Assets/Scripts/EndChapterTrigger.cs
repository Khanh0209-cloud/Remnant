using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndChapterTrigger : MonoBehaviour
{
    [Header("Loading Screen UI")]
    [Tooltip("Kéo Panel màn hình Loading (hoặc Canvas Loading) của bạn vào đây")]
    public GameObject loadingScreen;

    [Header("Cấu hình chuyển màn")]
    [Tooltip("Thời gian hiện màn hình chờ (giây) trước khi về Main Menu")]
    public float delayTime = 5f;

    private bool hasTriggered = false;

    private void Start()
    {
        // Đảm bảo màn hình loading luôn bị ẩn khi mới bắt đầu game
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng chạm vào có phải là Player không
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            
            // Tạm khóa di chuyển của người chơi
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.enabled = false;
                
                // Trả gia tốc về 0 để nhân vật dừng hẳn lại (không bị trượt đi tiếp)
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                }
                
                // Đưa nhân vật về trạng thái đứng im
                Animator anim = player.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetFloat("Speed", 0f);
                }
            }

            // Bắt đầu quy trình kết thúc chapter
            StartCoroutine(EndChapterRoutine());
        }
    }

    private IEnumerator EndChapterRoutine()
    {
        // Ép tắt ngay lập tức tiếng bước chân (nếu đang phát)
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetWalkingSound(false);
        }

        // 1. Bật màn hình Loading lên che toàn bộ game
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // 2. Yêu cầu AudioManager từ từ giảm âm lượng nhạc nền về 0
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StartFadeOutBGM(delayTime);
        }

        // 3. Đợi đúng 5 giây (hoặc thời gian delayTime bạn chỉnh)
        yield return new WaitForSeconds(delayTime);

        // 4. Chuyển scene về lại MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}

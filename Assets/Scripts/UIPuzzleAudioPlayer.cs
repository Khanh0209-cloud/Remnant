using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIPuzzleAudioPlayer : MonoBehaviour
{
    [Header("Cài đặt Âm thanh (Kéo file mp3/wav vào đây)")]
    [Tooltip("Bấm lần 1: Tiếng rè rè")]
    public AudioClip staticSound;
    
    [Tooltip("Bấm lần 2, 3, 4: Tiếng tua băng (Fast Forward)")]
    public AudioClip fastForwardSound;
    
    [Tooltip("Bấm lần 5 trở đi: Đoạn ghi âm thật sự")]
    public AudioClip recordingSound;
    
    [Tooltip("Âm lượng phát ra (từ 0 đến 1)")]
    [Range(0f, 1f)]
    public float volume = 1f;

    [Header("Phụ đề (Subtitle)")]
    [Tooltip("Kéo TextMeshPro chứa phụ đề vào đây")]
    public TMPro.TextMeshProUGUI subtitleText;
    
    [Header("Thời gian hiện từng câu Subtitle (giây)")]
    public float delayLine1 = 4f;
    public float delayLine2 = 4f;
    public float delayLine3 = 6f;

    private int clickCount = 0;
    private AudioSource audioSource;
    private Coroutine subtitleCoroutine;

    void Awake()
    {
        // Lấy AudioSource tự động được gắn kèm
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; 
        
        // Ẩn phụ đề lúc mới bắt đầu
        if (subtitleText != null)
        {
            subtitleText.gameObject.SetActive(false);
        }
    }

    // Hàm này sẽ được gọi khi bấm nút trên UI
    public void PlayPuzzleSound()
    {
        clickCount++;
        
        // 1. Chỉ phát tiếng "Cạch" vật lý ở 4 lần bấm đầu tiên để tránh bị lặp đè khi phát ghi âm
        if (AudioManager.Instance != null && clickCount < 5)
        {
            AudioManager.Instance.PlayTapCassette();
        }

        // 2. Cắt đứt NGAY LẬP TỨC mọi âm thanh rè/tua nhanh cũ đang chạy dở
        audioSource.Stop();

        AudioClip clipToPlay = null;

        // Trả tốc độ âm thanh (pitch) về bình thường mặc định
        audioSource.pitch = 1f;

        // Phân loại âm thanh dựa trên số lần bấm
        if (clickCount == 1)
        {
            clipToPlay = staticSound;
            Debug.Log("Lần 1: Phát tiếng rè rè...");
        }
        else if (clickCount >= 2 && clickCount <= 4)
        {
            clipToPlay = fastForwardSound;
            // Kích tốc độ chạy của file âm thanh lên nhanh gấp 2.5 lần để tạo tiếng tua vội
            audioSource.pitch = 2.5f; 
            Debug.Log("Lần " + clickCount + ": Phát tiếng tua nhanh...");
        }
        else // Lần 5 trở lên
        {
            clipToPlay = recordingSound;
            Debug.Log("Lần " + clickCount + ": Đang phát đoạn ghi âm gốc!");
            
            // Bật phụ đề và chạy từng câu (Coroutine)
            if (subtitleText != null)
            {
                subtitleText.gameObject.SetActive(true);
                
                // Cắt đứt phụ đề cũ nếu người chơi bấm nút liên tục
                if (subtitleCoroutine != null) StopCoroutine(subtitleCoroutine);
                
                subtitleCoroutine = StartCoroutine(PlaySubtitlesSequence());
            }
        }

        // Phát âm thanh
        if (clipToPlay != null)
        {
            audioSource.volume = volume;
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Bạn chưa kéo file âm thanh vào ô trống tương ứng cho lần bấm thứ " + clickCount);
        }
    }

    private System.Collections.IEnumerator PlaySubtitlesSequence()
    {
        // Câu 1
        subtitleText.text = "\"Today is <color=#FFFF00>February 12th</color>...\"";
        yield return new WaitForSeconds(delayLine1);

        // Câu 2
        subtitleText.text = "\"Happy birthday, my dear son.\"";
        yield return new WaitForSeconds(delayLine2);

        // Câu 3
        subtitleText.text = "\"My sweet boy... I hope you'll always stay healthy and grow up strong.\"";
        yield return new WaitForSeconds(delayLine3);

        // Tự động tắt phụ đề sau khi đọc xong
        subtitleText.text = "";
        subtitleText.gameObject.SetActive(false);
    }
}

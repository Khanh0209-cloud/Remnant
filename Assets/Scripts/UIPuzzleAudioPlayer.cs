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

    private int clickCount = 0;
    private AudioSource audioSource;

    void Awake()
    {
        // Lấy AudioSource tự động được gắn kèm
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; 
    }

    // Hàm này sẽ được gọi khi bấm nút trên UI
    public void PlayPuzzleSound()
    {
        clickCount++;
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
        }

        // Phát âm thanh
        if (clipToPlay != null)
        {
            audioSource.volume = volume;
            audioSource.Stop(); // Cắt đứt âm thanh cũ nếu người chơi bấm quá nhanh
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Bạn chưa kéo file âm thanh vào ô trống tương ứng cho lần bấm thứ " + clickCount);
        }
    }
}

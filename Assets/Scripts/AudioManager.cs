using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Fade Settings")]
    [Tooltip("Thời gian (giây) để nhạc nền to dần lên")]
    public float fadeInDuration = 2.0f;
    [Tooltip("Âm lượng tối đa của nhạc nền (0.0 đến 1.0)")]
    public float maxBgmVolume = 1.0f;

    private void Awake()
    {
        // Singleton pattern để dễ dàng gọi AudioManager từ bất kỳ script nào
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ AudioManager không bị hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (bgmSource != null && bgmSource.clip != null)
        {
            // Bắt đầu fade in âm thanh
            StartCoroutine(FadeInBGM());
        }
    }

    private IEnumerator FadeInBGM()
    {
        float currentTime = 0;
        bgmSource.volume = 0; // Bắt đầu từ mức âm lượng 0
        bgmSource.Play();

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            // Tăng dần âm lượng theo thời gian
            bgmSource.volume = Mathf.Lerp(0, maxBgmVolume, currentTime / fadeInDuration);
            yield return null; // Đợi đến frame tiếp theo
        }

        // Đảm bảo âm lượng đạt mức tối đa ở cuối
        bgmSource.volume = maxBgmVolume;
    }

    public void PlayHoverSound()
    {
        if (sfxSource != null && hoverSound != null)
        {
            sfxSource.PlayOneShot(hoverSound);
        }
    }

    public void PlayClickSound()
    {
        if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }
}

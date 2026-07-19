using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    [Tooltip("Âm lượng nhạc nền trong màn chơi chính (rất nhỏ)")]
    public float gameplayBgmVolume = 0.2f;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nếu vào màn chơi chính (SampleScene), bật lại nhạc ở mức rất nhỏ
        if (scene.name == "SampleScene")
        {
            if (bgmSource != null)
            {
                bgmSource.volume = gameplayBgmVolume;
                if (!bgmSource.isPlaying)
                {
                    bgmSource.Play();
                }
            }
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

    public void StartFadeOutBGM(float duration)
    {
        if (bgmSource != null)
        {
            StartCoroutine(FadeOutBGM(duration));
        }
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float currentTime = 0;
        float startVolume = bgmSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        bgmSource.volume = 0;
        bgmSource.Stop(); // Tắt hẳn nhạc
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

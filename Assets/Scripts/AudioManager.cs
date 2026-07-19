using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Nhạc nền (BGM)")]
    public AudioClip themeSong;
    public AudioSource bgmSource;
    
    [Header("Fade Settings (MainMenu)")]
    [Tooltip("Thời gian (giây) để nhạc nền to dần lên")]
    public float fadeInDuration = 2.0f;
    [Tooltip("Âm lượng tối đa của nhạc nền (0.0 đến 1.0)")]
    public float maxBgmVolume = 1.0f;
    [Tooltip("Âm lượng nhạc nền trong màn chơi chính (rất nhỏ)")]
    public float gameplayBgmVolume = 0.02f;

    [Header("Hiệu ứng âm thanh (SFX)")]
    public AudioClip walking_wooden;
    public AudioClip cassette_appear;
    public AudioClip open_cassette;
    public AudioClip pickup_paper;
    public AudioClip tap_cassette;
    public AudioClip oppened_vali;
    public AudioClip number_vali;
    public AudioClip invalid_vali_puzzle;
    public AudioClip solve_vali_puzzle;
    public AudioClip open_necklace;
    public AudioClip pickup_key;
    public AudioClip complete;
    
    [Header("Menu SFX")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public AudioSource sfxSource;
    private AudioSource footstepsSource;

    private void Awake()
    {
        // Mỗi màn chơi sẽ dùng một AudioManager riêng biệt với các file âm thanh được setup riêng cho màn đó.
        Instance = this;

        // Tạo kênh phát âm thanh riêng biệt nếu chưa kéo vào Inspector
        if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
        
        footstepsSource = gameObject.AddComponent<AudioSource>();

        // Cài đặt cho BGM (phát lặp lại vô tận)
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        
        // Cài đặt cho tiếng bước chân (phát lặp lại)
        footstepsSource.loop = true;
        footstepsSource.playOnAwake = false;
        
        // Cài đặt cho SFX (không lặp)
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
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
        // Nếu vào màn chơi chính (Chapter2), bật lại nhạc ở mức rất nhỏ
        if (scene.name == "Chapter2" || scene.name == "SampleScene")
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
        if (themeSong != null && bgmSource.clip == null)
        {
            bgmSource.clip = themeSong;
        }
        
        if (bgmSource != null && bgmSource.clip != null)
        {
            // Xác định mức âm lượng mục tiêu dựa vào Scene hiện tại đang đứng
            string currentScene = SceneManager.GetActiveScene().name;
            float targetVol = (currentScene == "Chapter2" || currentScene == "SampleScene") ? gameplayBgmVolume : maxBgmVolume;
            
            // Bắt đầu fade in âm thanh
            StartCoroutine(FadeInBGM(targetVol));
        }
    }

    public void PlayBGM(AudioClip clip, float volume = 0.5f)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.Play();
        }
    }

    private IEnumerator FadeInBGM(float targetVolume)
    {
        float currentTime = 0;
        bgmSource.volume = 0; 
        bgmSource.Play();

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, targetVolume, currentTime / fadeInDuration);
            yield return null;
        }
        bgmSource.volume = targetVolume;
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
        bgmSource.Stop(); 
    }

    public void PlayHoverSound()
    {
        if (sfxSource != null && hoverSound != null) sfxSource.PlayOneShot(hoverSound);
    }

    public void PlayClickSound()
    {
        if (sfxSource != null && clickSound != null) sfxSource.PlayOneShot(clickSound);
    }

    public void SetWalkingSound(bool isWalking)
    {
        if (walking_wooden == null) return;
        
        if (isWalking)
        {
            if (!footstepsSource.isPlaying)
            {
                footstepsSource.clip = walking_wooden;
                footstepsSource.Play();
            }
        }
        else
        {
            if (footstepsSource.isPlaying) footstepsSource.Stop();
        }
    }

    public void PlayCassetteAppear()
    {
        if (cassette_appear != null) sfxSource.PlayOneShot(cassette_appear);
    }

    public void PlayOpenCassette()
    {
        if (open_cassette != null) sfxSource.PlayOneShot(open_cassette);
    }

    public void PlayInvalidValiPuzzle()
    {
        if (invalid_vali_puzzle != null) sfxSource.PlayOneShot(invalid_vali_puzzle);
    }

    public void PlayOpenNecklace()
    {
        if (open_necklace != null) sfxSource.PlayOneShot(open_necklace);
    }

    public void PlayPickupPaper()
    {
        if (pickup_paper != null) sfxSource.PlayOneShot(pickup_paper);
    }

    public void PlayTapCassette()
    {
        if (tap_cassette != null) sfxSource.PlayOneShot(tap_cassette);
    }

    public void PlayOpenedVali()
    {
        if (oppened_vali != null) sfxSource.PlayOneShot(oppened_vali);
    }

    public void PlayNumberVali()
    {
        if (number_vali != null) sfxSource.PlayOneShot(number_vali);
    }

    public void PlaySolveValiPuzzle()
    {
        if (solve_vali_puzzle != null) sfxSource.PlayOneShot(solve_vali_puzzle);
    }

    public void PlayPickupKey()
    {
        if (pickup_key != null) sfxSource.PlayOneShot(pickup_key);
    }

    public void PlayCompleteWithDelay(float delayTime)
    {
        StartCoroutine(CompleteDelayRoutine(delayTime));
    }

    private IEnumerator CompleteDelayRoutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (complete != null) sfxSource.PlayOneShot(complete);
    }
}

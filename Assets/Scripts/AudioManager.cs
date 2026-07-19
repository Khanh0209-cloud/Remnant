using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Nhạc nền (BGM)")]
    public AudioClip themeSong;
    private AudioSource bgmSource;

    [Header("Hiệu ứng âm thanh (SFX)")]
    public AudioClip pickup_paper;
    public AudioClip tap_cassette;
    public AudioClip oppened_vali;
    public AudioClip number_vali;
    public AudioClip solve_vali_puzzle;
    public AudioClip pickup_key;
    public AudioClip complete;

    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Không phá hủy AudioManager khi chuyển Scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tạo 2 kênh phát âm thanh riêng biệt
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Cài đặt cho BGM (phát lặp lại vô tận)
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        
        // Cài đặt cho SFX (không lặp)
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        if (themeSong != null)
        {
            PlayBGM(themeSong);
        }
    }

    // ===== HÀM ĐIỀU KHIỂN NHẠC NỀN =====
    public void PlayBGM(AudioClip clip, float volume = 0.5f)
    {
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.Play();
        }
    }

    // ===== CÁC HÀM GỌI HIỆU ỨNG ÂM THANH =====

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

    // Hàm phát tiếng complete sau một khoảng thời gian chờ (delay)
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

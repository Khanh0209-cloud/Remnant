using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Kéo Panel hoặc Image chứa ảnh Options Demo vào đây")]
    public GameObject optionsPanel;
    
    [Header("New Panels")]
    public GameObject audioPanel;
    public GameObject guidePanel;
    public GameObject loadingPanel;

    void Start()
    {
        // Đảm bảo khi khởi động Main Menu thì tắt màn hình Options đi
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        if (audioPanel != null) audioPanel.SetActive(false);
        if (guidePanel != null) guidePanel.SetActive(false);
        if (loadingPanel != null) loadingPanel.SetActive(false);
    }

    // Hàm gọi khi nhấn nút Play
    public void PlayGame()
    {
        StartCoroutine(LoadSceneWithDelay("SampleScene", 5f));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delaySeconds)
    {
        // Hiện màn hình loading
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }

        // Chờ khoảng thời gian (ở đây là 2 giây)
        yield return new WaitForSeconds(delaySeconds);

        // Chuyển scene
        SceneManager.LoadScene(sceneName);
    }

    // Hàm gọi khi nhấn nút Options
    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    // Hàm gọi khi nhấn nút Close (nếu có) trên màn hình Options hoặc bấm ra ngoài
    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void OpenAudio()
    {
        if (audioPanel != null) audioPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void CloseAudio()
    {
        if (audioPanel != null) audioPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void OpenGuide()
    {
        if (guidePanel != null) guidePanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void CloseGuide()
    {
        if (guidePanel != null) guidePanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // Hàm gọi khi nhấn nút Exit
    public void ExitGame()
    {
        Debug.Log("Đang thoát game..."); // Dùng để kiểm tra trong Editor
        Application.Quit(); // Chỉ hoạt động khi đã build ra file .exe
    }
}

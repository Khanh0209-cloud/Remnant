using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Kéo Panel hoặc Image chứa ảnh Settings Demo vào đây")]
    public GameObject settingsPanel;

    void Start()
    {
        // Đảm bảo khi khởi động Main Menu thì tắt màn hình Settings đi
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Hàm gọi khi nhấn nút Play
    public void PlayGame()
    {
        // Chuyển sang scene SampleScene (nhớ thêm vào Build Settings)
        SceneManager.LoadScene("SampleScene");
    }

    // Hàm gọi khi nhấn nút Settings
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Hàm gọi khi nhấn nút Close (nếu có) trên màn hình Settings hoặc bấm ra ngoài
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Hàm gọi khi nhấn nút Exit
    public void ExitGame()
    {
        Debug.Log("Đang thoát game..."); // Dùng để kiểm tra trong Editor
        Application.Quit(); // Chỉ hoạt động khi đã build ra file .exe
    }
}

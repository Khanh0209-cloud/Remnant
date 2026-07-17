using UnityEngine;
using TMPro;

public class CombinationLockUI : MonoBehaviour
{
    [Header("Giao diện các vòng xoay")]
    [Tooltip("Kéo 4 cái chữ (Text) bên trong 4 nút xoay số vào đây")]
    public TextMeshProUGUI[] digitTexts; 
    
    // Lưu số hiện tại của 4 vòng xoay
    private int[] currentDigits = new int[4];
    
    // Mã đúng (1202)
    private int[] correctCode = { 1, 2, 0, 2 };

    [Header("Phần thưởng mở khóa")]
    public GameObject teddyBearPrefab;
    public Transform spawnPoint; // Tọa độ rơi gấu bông
    public PlayerMovement player; // Để mở khóa di chuyển

    void Start()
    {
        // Ban đầu các vòng xoay đều là 0
        UpdateDisplay();
    }

    // Hàm này sẽ được gán vào sự kiện OnClick của 4 nút bấm
    // Truyền index: 0 (nút đầu), 1 (nút 2), 2 (nút 3), 3 (nút 4)
    public void OnDigitClicked(int index)
    {
        currentDigits[index]++;
        if (currentDigits[index] > 9)
        {
            currentDigits[index] = 0; // Vượt 9 thì quay về 0
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < 4; i++)
        {
            if (digitTexts.Length > i && digitTexts[i] != null)
            {
                digitTexts[i].text = currentDigits[i].ToString();
            }
        }
    }

    // Hàm này gán vào sự kiện OnClick của nút Btn_Unlock
    public void TryUnlock()
    {
        bool isCorrect = true;
        for (int i = 0; i < 4; i++)
        {
            if (currentDigits[i] != correctCode[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Suitcase: Bấm tách! Mở khóa Vali THÀNH CÔNG!");
            
            if (teddyBearPrefab != null && spawnPoint != null)
            {
                Instantiate(teddyBearPrefab, spawnPoint.position, Quaternion.identity);
            }
            
            // Đóng bảng UI lại
            CloseUI();
        }
        else
        {
            Debug.Log("Suitcase: Sai mật mã, ổ khóa không nhúc nhích!");
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        if (player != null)
        {
            player.enabled = true; // Mở lại quyền di chuyển cho nhân vật
        }
    }
}

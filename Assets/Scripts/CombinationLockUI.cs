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
    public SpriteRenderer suitcaseSpriteRenderer;
    public Sprite openSuitcaseSprite;
    public SuitcaseInteractable oldSuitcaseInteractable;
    public SuitcaseOpenedInteractable newSuitcaseOpenedInteractable;
    public PlayerMovement player;

    void Start()
    {
        UpdateDisplay();
    }

    public void OnDigitClicked(int index)
    {
        currentDigits[index]++;
        if (currentDigits[index] > 9)
        {
            currentDigits[index] = 0;
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
            Debug.Log("Suitcase: Mở khóa Vali THÀNH CÔNG!");
            
            // Đổi hình vali sang trạng thái mở
            if (suitcaseSpriteRenderer != null && openSuitcaseSprite != null)
            {
                suitcaseSpriteRenderer.sprite = openSuitcaseSprite;
            }
            
            // Tắt chức năng mở UI Passcode, bật chức năng mở UI Mini-game Gấu Bông
            if (oldSuitcaseInteractable != null) oldSuitcaseInteractable.enabled = false;
            if (newSuitcaseOpenedInteractable != null) newSuitcaseOpenedInteractable.enabled = true;
            
            CloseUI();
        }
        else
        {
            Debug.Log("Suitcase: Sai mật mã, ổ khóa không nhúc nhích!");
            
            // Báo sai: Rung màn hình
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
            if (cam != null)
            {
                cam.TriggerShake(0.3f, 0.1f);
            }
            
            // Reset các số về 0
            for (int i = 0; i < 4; i++)
            {
                currentDigits[i] = 0;
            }
            UpdateDisplay();
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        if (player != null)
        {
            player.enabled = true;
        }
    }
}

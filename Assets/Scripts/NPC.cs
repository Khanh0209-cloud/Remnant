using UnityEngine;
using TMPro; // Sử dụng TextMesh Pro đi kèm UGUI 2.0 trong Unity 6

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Profile")]
    public string npcName = "Mion";

    [Header("Dialogue Settings")]
    [TextArea(3, 5)]
    public string[] dialogueLines;
    
    [Header("UI References")]
    [Tooltip("Kéo Panel chứa giao diện đối thoại vào đây")]
    public GameObject dialoguePanel;
    
    [Tooltip("Kéo TextMeshPro hiển thị nội dung lời thoại vào đây")]
    public TextMeshProUGUI dialogueText;
    
    [Tooltip("Kéo TextMeshPro hiển thị tên NPC vào đây (Không bắt buộc)")]
    public TextMeshProUGUI nameText;

    private int currentLineIndex = 0;
    private bool isInteracting = false;
    private Transform playerTransform;

    void Start()
    {
        // Tự động ẩn Panel hội thoại khi bắt đầu game
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Nếu đang đối thoại và nhấn F hoặc Space để xem câu thoại tiếp theo
        if (isInteracting && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space)))
        {
            NextLine();
        }

        // Tự động đóng đối thoại nếu người chơi di chuyển ra quá xa NPC
        if (isInteracting && playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance > 2.5f) 
            {
                EndDialogue();
            }
        }
    }

    // Hàm thực thi được gọi từ PlayerMovement khi đứng gần NPC và nhấn phím F
    public void Interact()
    {
        if (isInteracting) return;

        // Tìm kiếm đối tượng Player trong Scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        StartDialogue();
    }

    private void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning($"{npcName} chưa được thiết lập lời thoại nào!");
            return;
        }

        isInteracting = true;
        currentLineIndex = 0;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        if (nameText != null)
        {
            nameText.text = npcName;
        }

        ShowLine();
    }

    private void ShowLine()
    {
        if (dialogueText != null)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
        else
        {
            // Dự phòng ghi Log ra Console nếu chưa kéo thả UI
            Debug.Log($"[{npcName}]: {dialogueLines[currentLineIndex]}");
        }
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        isInteracting = false;
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}

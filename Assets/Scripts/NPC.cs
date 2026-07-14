using UnityEngine;
using TMPro;

[System.Serializable]
public struct DialogueLine
{
    public string speakerName;
    [TextArea(3, 5)]
    public string text;
}

public class NPC : MonoBehaviour, IInteractable
{
    [Header("NPC Profile")]
    public string npcName = "Mion";

    [Header("Dialogue Phase 1: Before Quest")]
    public DialogueLine[] dialoguePhase1;

    [Header("Dialogue Phase 2: After Quest")]
    public DialogueLine[] dialoguePhase2;
    
    [Header("UI References")]
    [Tooltip("Kéo Panel chứa giao diện đối thoại vào đây")]
    public GameObject dialoguePanel;
    
    [Tooltip("Kéo TextMeshPro hiển thị nội dung lời thoại vào đây")]
    public TextMeshProUGUI dialogueText;
    
    [Tooltip("Kéo TextMeshPro hiển thị tên người nói vào đây (Không bắt buộc)")]
    public TextMeshProUGUI nameText;

    private DialogueLine[] currentDialogue;
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
        // CHỈ Click chuột trái để chuyển sang câu thoại tiếp theo
        if (isInteracting && Input.GetMouseButtonDown(0))
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

    // Hàm click chuột trái vào nhân vật Mion để xem lại đoạn hội thoại (nếu đang ở gần)
    void OnMouseDown()
    {
        if (isInteracting) return;

        // Tìm kiếm đối tượng Player trong Scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= 2.5f) // Chỉ cho phép xem lại hội thoại khi đứng gần
            {
                playerTransform = player.transform;
                StartDialogue(player.GetComponent<PlayerMovement>());
            }
        }
    }

    // Hàm thực thi tương tác phím F (bắt nguồn từ PlayerMovement)
    public void Interact()
    {
        if (isInteracting) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            StartDialogue(player.GetComponent<PlayerMovement>());
        }
    }

    private void StartDialogue(PlayerMovement playerScript)
    {
        // Kiểm tra xem người chơi đã có đồng hồ chưa để chọn mảng thoại phù hợp
        bool hasWatch = false;
        if (playerScript != null)
        {
            hasWatch = playerScript.hasWatch;
        }

        currentDialogue = hasWatch ? dialoguePhase2 : dialoguePhase1;

        if (currentDialogue == null || currentDialogue.Length == 0)
        {
            Debug.LogWarning($"{npcName} chưa được thiết lập lời thoại cho giai đoạn này!");
            return;
        }

        isInteracting = true;
        currentLineIndex = 0;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        ShowLine();
    }

    private void ShowLine()
    {
        DialogueLine currentLine = currentDialogue[currentLineIndex];

        // Cập nhật lời thoại
        if (dialogueText != null)
        {
            dialogueText.text = currentLine.text;
        }
        else
        {
            Debug.Log($"[{currentLine.speakerName}]: {currentLine.text}");
        }

        // Cập nhật tên người nói luân phiên
        if (nameText != null)
        {
            nameText.text = currentLine.speakerName;
        }
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < currentDialogue.Length)
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

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
    
    [Header("UI References (NPC - Bên Phải)")]
    public GameObject rightDialoguePanel;
    public TextMeshProUGUI rightDialogueText;
    public TextMeshProUGUI rightNameText;

    [Header("UI References (Player - Bên Trái)")]
    public GameObject leftDialoguePanel;
    public TextMeshProUGUI leftDialogueText;
    public TextMeshProUGUI leftNameText;

    [Header("Name Tag Setup")]
    [Tooltip("Điền tên nhân vật chính xác để hệ thống bật Panel bên trái")]
    public string playerName = "Nox";

    [Header("Quest Settings")]
    [Tooltip("Bức tường tàng hình ngăn qua màn (Sẽ tự động biến mất khi xong nhiệm vụ)")]
    public GameObject levelBlocker;

    private DialogueLine[] currentDialogue;
    private int currentLineIndex = 0;
    private bool isInteracting = false;
    private Transform playerTransform;
    private PlayerMovement currentPlayerScript;
    private int startDialogueFrame = -1;

    [System.NonSerialized] public bool hasSpokenPhase1 = false;

    void Start()
    {
        // Tự động ẩn Panel hội thoại khi bắt đầu game
        if (rightDialoguePanel != null) rightDialoguePanel.SetActive(false);
        if (leftDialoguePanel != null) leftDialoguePanel.SetActive(false);
    }

    void Update()
    {
        // CHỈ Click chuột trái để chuyển sang câu thoại tiếp theo
        if (isInteracting && Input.GetMouseButtonDown(0) && Time.frameCount > startDialogueFrame)
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
        // Lưu lại script của người chơi để lát nữa mở khóa di chuyển
        currentPlayerScript = playerScript;

        // Khóa di chuyển của người chơi
        if (currentPlayerScript != null)
        {
            currentPlayerScript.canMove = false;
        }

        // Kiểm tra xem người chơi đã có đồng hồ chưa để chọn mảng thoại phù hợp
        bool hasWatch = false;
        if (playerScript != null)
        {
            hasWatch = playerScript.hasWatch;
        }

        // code cũ ngay dưới này là currentDialogue = hasWatch ? dialoguePhase2 : dialoguePhase1;
        Debug.Log($"[NPC Debug] hasSpokenPhase1 = {hasSpokenPhase1} | hasWatch = {hasWatch}");
        if (hasSpokenPhase1 == false)
        {
            currentDialogue = dialoguePhase1;
        }
        else if (hasSpokenPhase1 == true && hasWatch == true)
        {
            currentDialogue = dialoguePhase2;
        }
        else
        {
            currentDialogue = dialoguePhase1;
        }

        if (currentDialogue == null || currentDialogue.Length == 0)
        {
            Debug.LogWarning($"{npcName} chưa được thiết lập lời thoại cho giai đoạn này!");
            
            // Nếu lỗi, mở khóa lại ngay lập tức
            if (currentPlayerScript != null) currentPlayerScript.canMove = true;
            return;
        }

        isInteracting = true;
        currentLineIndex = 0;
        startDialogueFrame = Time.frameCount;

        ShowLine();
    }

    private void ShowLine()
    {
        DialogueLine currentLine = currentDialogue[currentLineIndex];

        if (currentLine.speakerName == playerName)
        {
            // Bật bên trái, tắt bên phải
            if (leftDialoguePanel != null) leftDialoguePanel.SetActive(true);
            if (rightDialoguePanel != null) rightDialoguePanel.SetActive(false);

            if (leftDialogueText != null) leftDialogueText.text = currentLine.text;
            if (leftNameText != null) leftNameText.text = currentLine.speakerName;
        }
        else
        {
            // Bật bên phải, tắt bên trái
            if (rightDialoguePanel != null) rightDialoguePanel.SetActive(true);
            if (leftDialoguePanel != null) leftDialoguePanel.SetActive(false);

            if (rightDialogueText != null) rightDialogueText.text = currentLine.text;
            if (rightNameText != null) rightNameText.text = currentLine.speakerName;
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
        
        if (rightDialoguePanel != null) rightDialoguePanel.SetActive(false);
        if (leftDialoguePanel != null) leftDialoguePanel.SetActive(false);

        // --- 1. THÊM ĐOẠN NÀY ĐỂ ĐÁNH DẤU LÀ ĐÃ NÓI CHUYỆN ---
        if (currentDialogue == dialoguePhase1)
        {
            hasSpokenPhase1 = true;
        }

        if (currentPlayerScript != null)
        {
            // KHOẢN THƯỞNG NHIỆM VỤ: Mở đường nếu đã có đồng hồ (Nói chuyện xong Phase 2)
            if (currentDialogue == dialoguePhase2 && currentPlayerScript.hasWatch && levelBlocker != null)
            {
                levelBlocker.SetActive(false);
                Debug.Log("Đã mở khóa đường đi tiếp!");
            }

            // Mở khóa di chuyển lại cho người chơi
            currentPlayerScript.canMove = true;
            currentPlayerScript = null; // Trả lại trạng thái trống
        }
    }
}

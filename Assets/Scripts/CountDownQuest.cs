using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownQuest : Interactable
{
    [Header("Quest Settings")]
    public float timeLimit = 30f;
    public GameObject targetItem;

    [Header("NPC & Animation Settings")]
    public GameObject npcObject; // Đảm bảo đã kéo thả đối tượng NPC (Hazel) vào đây
    public Animator playerAnimator;
    public string executionTrigger = "Die";

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public GameObject dialogueUI;

    [Header("Left Side UI Elements")]
    public GameObject leftDialogueBox;
    public TextMeshProUGUI leftNameText;
    public TextMeshProUGUI leftDialogueText;

    [Header("Right Side UI Elements")]
    public GameObject rightDialogueBox;
    public TextMeshProUGUI rightNameText;
    public TextMeshProUGUI rightDialogueText;

    [Header("1. Start Dialogue Content")]
    public DialogueLine[] startLines;

    [Header("2. Success Dialogue Content")]
    public DialogueLine[] successLines;

    private int currentLine = 0;
    private float timeRemaining;

    private bool isTalkingStart = false;
    private bool isQuestActive = false;
    private bool isQuestCompleted = false;
    private bool isInSuccessDialogue = false;

    // Ẩn toàn bộ UI liên quan khi game vừa khởi động
    void Start()
    {
        if (dialogueUI != null) dialogueUI.SetActive(false);
        if (timerText != null) timerText.gameObject.SetActive(false);

        // Ép Animator không chạy bậy lúc đầu game
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger(executionTrigger);
        }
    }

    void Update()
    {
        if (isQuestActive && !isQuestCompleted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                QuestFailed();
            }
        }
    }

    public override void Interact()
    {
        if (isInSuccessDialogue)
        {
            NextSuccessLine();
            return;
        }

        if (isTalkingStart)
        {
            NextStartLine();
            return;
        }

        if (!isQuestActive && !isQuestCompleted)
        {
            StartStartDialogue();
            return;
        }

        if (isQuestActive && !isQuestCompleted)
        {
            if (targetItem != null && !targetItem.activeSelf)
            {
                StartSuccessDialogue();
            }
            else
            {
                Debug.Log("Hazel: 'Thịt của tôi đâu? Nhanh lên trước khi hết giờ!'");
            }
        }
    }

    // ================= LOOP HỘI THOẠI MỞ ĐẦU =================
    void StartStartDialogue()
    {
        isTalkingStart = true;
        currentLine = 0;
        if (dialogueUI != null) dialogueUI.SetActive(true);
        ShowLine(startLines);
    }

    void NextStartLine()
    {
        currentLine++;
        if (startLines == null || currentLine >= startLines.Length)
        {
            isTalkingStart = false;
            if (dialogueUI != null) dialogueUI.SetActive(false);

            StartQuest();
            return;
        }
        ShowLine(startLines);
    }

    void StartQuest()
    {
        isQuestActive = true;
        timeRemaining = timeLimit;
        if (timerText != null) timerText.gameObject.SetActive(true);
        if (targetItem != null) targetItem.SetActive(true);
    }

    // ================= LOOP HỘI THOẠI THÀNH CÔNG =================
    void StartSuccessDialogue()
    {
        isQuestActive = false;
        isQuestCompleted = true;
        isInSuccessDialogue = true;
        currentLine = 0;

        if (timerText != null) timerText.gameObject.SetActive(false);
        if (dialogueUI != null) dialogueUI.SetActive(true);

        ShowLine(successLines);
    }

    void NextSuccessLine()
    {
        currentLine++;
        if (successLines == null || currentLine >= successLines.Length)
        {
            isInSuccessDialogue = false;
            if (dialogueUI != null) dialogueUI.SetActive(false);

            // --- TRƯỜNG HỢP 1: ẨN HAZEL KHI HOÀN THÀNH NHIỆM VỤ THÀNH CÔNG ---
            if (npcObject != null)
            {
                npcObject.SetActive(false);
            }

            Debug.Log("🎉 Thành công: Hội thoại kết thúc và Hazel đã ẩn.");
            return;
        }
        ShowLine(successLines);
    }

    void ShowLine(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0 || currentLine >= lines.Length) return;

        DialogueLine line = lines[currentLine];

        if (line.isLeft)
        {
            leftDialogueBox.SetActive(true);
            rightDialogueBox.SetActive(false);
            leftNameText.text = line.speaker;
            leftDialogueText.text = line.text;
        }
        else
        {
            leftDialogueBox.SetActive(false);
            rightDialogueBox.SetActive(true);
            rightNameText.text = line.speaker;
            rightDialogueText.text = line.text;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time left: " + Mathf.CeilToInt(timeRemaining).ToString() + "s";
        }
    }

    void QuestFailed()
    {
        isQuestActive = false;
        isQuestCompleted = true;
        if (timerText != null) timerText.text = "There is no more, foxy";
        if (targetItem != null) targetItem.SetActive(false);

        // --- TRƯỜNG HỢP 2: ẨN NGAY HAZEL BẢN GỐC KHI THẤT BẠI (TRƯỚC KHI CẮN) ---
        if (npcObject != null)
        {
            npcObject.SetActive(false);
        }

        // Sau khi Hazel đứng yên biến mất, hoạt ảnh trồi lên cắn của Nox sẽ được kích hoạt sạch sẽ
        if (playerAnimator != null) playerAnimator.SetTrigger(executionTrigger);

        Invoke("RestartScene", 6.5f);
    }

    void RestartScene()
    {
        if (timerText != null) timerText.gameObject.SetActive(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
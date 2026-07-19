using UnityEngine;
using TMPro;

public class NPCDialogue : Interactable
{
    [Header("Dialogue UI Controller")]
    public GameObject dialogueUI;

    [Header("Left Side Elements")]
    public GameObject leftDialogueBox;
    public TextMeshProUGUI leftNameText;
    public TextMeshProUGUI leftDialogueText;

    [Header("Right Side Elements")]
    public GameObject rightDialogueBox;
    public TextMeshProUGUI rightNameText;
    public TextMeshProUGUI rightDialogueText;

    [Header("Dialogue Content")]
    public DialogueLine[] dialogueLines;

    private int currentLine = 0;
    private bool isTalking = false;

    void Start()
    {
        if (dialogueUI != null) dialogueUI.SetActive(false);
    }

    public override void Interact()
    {
        // Kiểm tra xem đã kéo đủ các ô trong Inspector chưa
        if (dialogueUI == null || leftDialogueBox == null || rightDialogueBox == null ||
            leftNameText == null || leftDialogueText == null ||
            rightNameText == null || rightDialogueText == null)
        {
            Debug.LogError("❌ Thiếu liên kết UI! Hãy kiểm tra lại các ô trống trong Inspector.");
            return;
        }

        if (!isTalking) StartDialogue();
        else NextLine();
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        dialogueUI.SetActive(true);
        ShowLine();
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }
        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = dialogueLines[currentLine];

        if (line.isLeft)
        {
            // Bật khung trái, ẩn khung phải
            leftDialogueBox.SetActive(true);
            rightDialogueBox.SetActive(false);

            // Gán dữ liệu vào các text bên trái
            leftNameText.text = line.speaker;
            leftDialogueText.text = line.text;
        }
        else
        {
            // Bật khung phải, ẩn khung trái
            leftDialogueBox.SetActive(false);
            rightDialogueBox.SetActive(true);

            // Gán dữ liệu vào các text bên phải
            rightNameText.text = line.speaker;
            rightDialogueText.text = line.text;
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);

        // THÊM DÒNG NÀY: Tự động tìm bộ đếm ngược trên cùng NPC và kích hoạt nó
        CountdownQuest quest = GetComponent<CountdownQuest>();
        if (quest != null)
        {
            quest.Interact();
        }
    }
}
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct DialogueLine
{
    public bool isLeftSpeaker; // true: Nox (Trái), false: NPC (Phải)
    public string speakerName;
    [TextArea(2, 5)]
    public string sentence;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Khung thoại Trái (Nox)")]
    public GameObject leftChatPanel;
    public TextMeshProUGUI leftNameText;
    public TextMeshProUGUI leftDialogueText;

    [Header("Khung thoại Phải (NPC)")]
    public GameObject rightChatPanel;
    public TextMeshProUGUI rightNameText;
    public TextMeshProUGUI rightDialogueText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        
        leftChatPanel.SetActive(false);
        rightChatPanel.SetActive(false);
    }

    // Hàm gọi Hội thoại, sẽ khóa di chuyển của Player cho đến khi chạy xong
    public IEnumerator PlayDialogueCoroutine(List<DialogueLine> lines, PlayerMovement player)
    {
        if (player != null) player.enabled = false; // Khóa di chuyển

        // Đợi người chơi nhả phím F ra (tránh tình trạng vừa bấm F để tương tác thì nó nhảy luôn câu thoại 1)
        yield return null; 
        while (Input.GetKey(KeyCode.F)) yield return null;

        foreach (DialogueLine line in lines)
        {
            // Bật/tắt đúng bảng thoại
            leftChatPanel.SetActive(line.isLeftSpeaker);
            rightChatPanel.SetActive(!line.isLeftSpeaker);

            // Cập nhật tên và chữ
            if (line.isLeftSpeaker)
            {
                if (leftNameText != null) leftNameText.text = line.speakerName;
                if (leftDialogueText != null) leftDialogueText.text = line.sentence;
            }
            else
            {
                if (rightNameText != null) rightNameText.text = line.speakerName;
                if (rightDialogueText != null) rightDialogueText.text = line.sentence;
            }

            // Dừng chờ người chơi bấm phím F
            while (!Input.GetKeyDown(KeyCode.F))
            {
                yield return null;
            }
            
            // Chờ người chơi nhả phím F ra để đọc câu tiếp theo
            yield return null;
            while (Input.GetKey(KeyCode.F)) yield return null;
        }

        // Chạy xong hết các câu thoại thì ẩn khung chat đi
        leftChatPanel.SetActive(false);
        rightChatPanel.SetActive(false);
        
        // Đợi 1 frame để tránh dính phím F làm tương tác lại ngay lập tức
        yield return null;

        if (player != null) player.enabled = true; // Mở khóa di chuyển
    }
}

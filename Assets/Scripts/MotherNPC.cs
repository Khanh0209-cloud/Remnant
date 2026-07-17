using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotherNPC : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo vật thể NPC Người Con (đang ẩn) vào đây")]
    public GameObject sonNPC;
    
    [Tooltip("Kéo nhân vật Nox vào đây để khóa di chuyển khi nói chuyện")]
    public PlayerMovement player;
    
    [Header("Kịch bản thoại 1: Lần đầu gặp")]
    public List<ChatLine> firstMeetingLines;
    
    [Header("Kịch bản thoại 2: Đưa dây chuyền (Trước khi con xuất hiện)")]
    public List<ChatLine> afterNecklacePart1Lines;
    
    [Header("Kịch bản thoại 3: Đưa dây chuyền (Con đã ôm mẹ)")]
    public List<ChatLine> afterNecklacePart2Lines;

    private bool isTalking = false;
    private bool hasFinishedCutscene = false;

    public void Interact()
    {
        // Chặn tương tác nếu đang nói chuyện hoặc đã qua màn
        if (isTalking || hasFinishedCutscene) return;

        if (Chapter2Manager.Instance != null && Chapter2Manager.Instance.hasNecklace)
        {
            // Trường hợp 2: Đã nhặt được dây chuyền
            StartCoroutine(EndingCutsceneRoutine());
        }
        else
        {
            // Trường hợp 1: Mới gặp lần đầu
            StartCoroutine(FirstMeetingRoutine());
        }
    }

    private IEnumerator FirstMeetingRoutine()
    {
        isTalking = true;
        
        // Giao quyền cho DialogueManager và Dừng chờ cho đến khi đọc thoại xong
        yield return StartCoroutine(DialogueManager.Instance.PlayDialogueCoroutine(firstMeetingLines, player));
        
        // Đọc xong thì cho phép người chơi nhặt giấy
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.hasTalkedToMother = true;
        }
        
        isTalking = false;
    }

    private IEnumerator EndingCutsceneRoutine()
    {
        isTalking = true;
        hasFinishedCutscene = true; // Khóa vĩnh viễn không cho tương tác nữa
        
        // 1. Chạy hội thoại phần 1 (Nox: I found this... -> Mẹ: This is our picture...)
        yield return StartCoroutine(DialogueManager.Instance.PlayDialogueCoroutine(afterNecklacePart1Lines, player));
        
        // 2. Chèn cảnh Cutscene: Con xuất hiện
        Debug.Log("(Mặt dây chuyền phát sáng. Cậu con trai xuất hiện và ôm lấy mẹ.)");
        if (sonNPC != null) sonNPC.SetActive(true);
        
        // Chờ 1.5 giây để người chơi nhìn thấy thằng bé ôm mẹ
        yield return new WaitForSeconds(1.5f);
        
        // 3. Chạy hội thoại phần 2 (Con: Mom... -> Mẹ: Thank you Nox)
        yield return StartCoroutine(DialogueManager.Instance.PlayDialogueCoroutine(afterNecklacePart2Lines, player));
        
        // 4. Kết thúc: Mẹ con tan biến, phá vỡ Màn chắn
        Debug.Log("(Hai mẹ con tan thành những hạt sáng...)");
        
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.isLevelPassed = true;
            if (Chapter2Manager.Instance.levelBlocker != null)
            {
                Destroy(Chapter2Manager.Instance.levelBlocker);
                Debug.Log("Màn chắn cửa đã vỡ vụn!");
            }
        }
        
        if (sonNPC != null) sonNPC.SetActive(false);
        gameObject.SetActive(false);
        
        isTalking = false;
    }
}

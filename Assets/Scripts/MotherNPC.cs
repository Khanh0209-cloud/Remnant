using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotherNPC : MonoBehaviour, IInteractable
{
    [Tooltip("Kéo nhân vật Nox vào đây để khóa di chuyển khi nói chuyện")]
    public PlayerMovement player;
    
    [Header("Kịch bản thoại 1: Lần đầu gặp")]
    public List<ChatLine> firstMeetingLines;
    
    [Header("Kịch bản thoại 2: Đưa dây chuyền (Trước khi con xuất hiện)")]
    public List<ChatLine> afterNecklacePart1Lines;
    
    [Header("Kịch bản thoại 3: Đưa dây chuyền (Con đã ôm mẹ)")]
    public List<ChatLine> afterNecklacePart2Lines;

    [Header("Hoạt ảnh (Animation)")]
    public Animator anim;
    [Tooltip("Thời gian chạy animation HugSon (giây)")]
    public float hugDuration = 2f;
    [Tooltip("Thời gian chạy animation Gone (giây)")]
    public float goneDuration = 1.5f;
    [Tooltip("Tích vào đây để đảo ngược chiều ngang (trục X) từ lúc bắt đầu ôm con")]
    public bool flipHugAnimationX = true;
    [Tooltip("Dịch chuyển nhân vật lên/xuống (trục Y) ngay khi phát animation Gone để bù lỗi lệch hình")]
    public float goneOffsetY = 0.5f;

    private bool isTalking = false;
    private bool hasFinishedCutscene = false;
    
    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    // Được gọi bởi MotherTriggerZone khi người chơi đi ngang qua
    public void Appear()
    {
        gameObject.SetActive(true);
        if (anim != null) anim.SetTrigger("Appear");
        Debug.Log("NPC Mẹ đã xuất hiện!");
    }

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
        
        // 2. Chạy Animation Mẹ ôm con (Hoạt ảnh con chạy ra ôm nằm chung trong cục Animation này)
        Debug.Log("Mẹ phát animation HugSon (Bao gồm cả Con xuất hiện)...");
        
        // Xử lý đảo hướng từ lúc bắt đầu ôm con
        if (flipHugAnimationX)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = !sr.flipX;
            }
        }
        
        if (anim != null) anim.SetTrigger("HugSon");
        
        // Chờ hết thời gian animation HugSon
        yield return new WaitForSeconds(hugDuration);
        
        // 3. Chạy hội thoại phần 2 (Con: Mom... -> Mẹ: Thank you Nox)
        yield return StartCoroutine(DialogueManager.Instance.PlayDialogueCoroutine(afterNecklacePart2Lines, player));
        
        // 4. Chạy Animation Gone
        Debug.Log("Mẹ phát animation Gone...");
        
        // Bù trừ lỗi lệch tọa độ lập tức
        transform.position += new Vector3(0f, goneOffsetY, 0f);
        
        if (anim != null) anim.SetTrigger("Gone");
        
        // Chờ hết thời gian animation Gone
        yield return new WaitForSeconds(goneDuration);
        
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.isLevelPassed = true;
            if (Chapter2Manager.Instance.levelBlocker != null)
            {
                Destroy(Chapter2Manager.Instance.levelBlocker);
                Debug.Log("Màn chắn cửa đã vỡ vụn!");
            }
        }
        
        // Ẩn vật thể
        gameObject.SetActive(false);
        
        isTalking = false;
    }
}

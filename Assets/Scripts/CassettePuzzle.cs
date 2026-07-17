using UnityEngine;

public class CassettePuzzle : MonoBehaviour, IInteractable
{
    private int interactCount = 0;

    public void Interact()
    {
        interactCount++;
        
        switch (interactCount)
        {
            case 1:
                Debug.Log("Cassette: Đang Play... *Tiếng rè rè* ... Không nghe thấy gì.");
                break;
            case 2:
            case 3:
            case 4:
                Debug.Log("Cassette: Đang Fast Forward... (Tua nhanh).");
                break;
            case 5:
                Debug.Log("Cassette: Đang phát đoạn ghi âm... 'Hôm nay là ngày 12 tháng 2...' -> Mật mã là 1202!");
                break;
            default:
                Debug.Log("Cassette: Đoạn băng đã kết thúc.");
                break;
        }
    }
}

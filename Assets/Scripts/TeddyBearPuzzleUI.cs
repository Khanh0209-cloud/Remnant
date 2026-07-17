using UnityEngine;
using System.Collections;

public class TeddyBearPuzzleUI : MonoBehaviour
{
    [Header("Các thành phần UI")]
    public GameObject bearFrontObj;
    public GameObject bearBackLeftObj;
    public GameObject bearBackRightObj;
    public GameObject stuffingObj;
    public GameObject necklaceSmallObj;
    public GameObject necklaceZoomClosedObj;
    public GameObject necklaceZoomOpenObj;
    
    public PlayerMovement player;

    void OnEnable()
    {
        // Khi bảng UI bật lên, luôn reset về State 0 (Gấu mặt trước)
        ResetPuzzle();
    }

    public void ResetPuzzle()
    {
        bearFrontObj.SetActive(true);
        bearBackLeftObj.SetActive(false);
        bearBackRightObj.SetActive(false);
        stuffingObj.SetActive(false);
        necklaceSmallObj.SetActive(false);
        necklaceZoomClosedObj.SetActive(false);
        necklaceZoomOpenObj.SetActive(false);
    }

    // Gắn vào sự kiện OnClick của nút Gấu Mặt Trước
    public void ClickBearFront()
    {
        Debug.Log("Đã lật con gấu lại!");
        bearFrontObj.SetActive(false);
        bearBackLeftObj.SetActive(true);
        bearBackRightObj.SetActive(true);
        // Bông và Dây chuyền chưa hiện, chờ người chơi Xé (Kéo) 2 nửa lưng
    }

    // Hàm này sẽ được script DragToTear gọi khi cả 2 nửa bị xé ra đủ xa
    public void OnBearTornApart()
    {
        Debug.Log("Đã xé lưng gấu, rơi ra bông và dây chuyền!");
        stuffingObj.SetActive(true);
        necklaceSmallObj.SetActive(true);
    }

    // Gắn vào sự kiện OnClick của nút Dây chuyền nhỏ
    public void ClickNecklaceSmall()
    {
        Debug.Log("Nhặt dây chuyền lên ngắm (Zoom in)!");
        bearBackLeftObj.SetActive(false);
        bearBackRightObj.SetActive(false);
        stuffingObj.SetActive(false);
        necklaceSmallObj.SetActive(false);
        
        necklaceZoomClosedObj.SetActive(true);
    }

    // Gắn vào sự kiện OnClick của nút Dây chuyền Zoom Closed
    public void ClickNecklaceClosed()
    {
        Debug.Log("Mở mặt dây chuyền ra...!");
        necklaceZoomClosedObj.SetActive(false);
        necklaceZoomOpenObj.SetActive(true);
        
        // Đã hoàn thành mini-game, ghi nhận lấy được dây chuyền và tắt UI sau 2 giây
        if (Chapter2Manager.Instance != null)
        {
            Chapter2Manager.Instance.hasNecklace = true;
        }
        
        StartCoroutine(FinishPuzzleRoutine());
    }

    private IEnumerator FinishPuzzleRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false); // Tắt UI
        if (player != null) player.enabled = true; // Mở lại di chuyển
        
        Debug.Log("Hãy mang dây chuyền đến cho Mẹ!");
    }
}

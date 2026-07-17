using UnityEngine;
using System.Collections;

public class TeddyBearPuzzleUI : MonoBehaviour
{
    [Header("Các thành phần UI")]
    public GameObject bearFrontObj;
    public GameObject rotateButtonObj; // Nút lật gấu mới thêm
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
        if (rotateButtonObj != null) rotateButtonObj.SetActive(true);
        bearBackLeftObj.SetActive(false);
        bearBackRightObj.SetActive(false);
        stuffingObj.SetActive(false);
        necklaceSmallObj.SetActive(false);
        necklaceZoomClosedObj.SetActive(false);
        necklaceZoomOpenObj.SetActive(false);
    }

    // Gắn vào sự kiện OnClick của nút Rotate
    public void ClickRotateButton()
    {
        Debug.Log("Đã bấm nút Rotate để lật con gấu lại!");
        bearFrontObj.SetActive(false);
        if (rotateButtonObj != null) rotateButtonObj.SetActive(false);
        bearBackLeftObj.SetActive(true);
        bearBackRightObj.SetActive(true);
        
        // Bật sẵn Bông và Dây chuyền nằm chìm bên dưới 2 nửa lưng gấu
        stuffingObj.SetActive(true);
        necklaceSmallObj.SetActive(true);
    }

    // Hàm này không cần bật gì thêm vì đồ vật đã hiển thị sẵn bên dưới
    public void OnBearTornApart()
    {
        // Để trống
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

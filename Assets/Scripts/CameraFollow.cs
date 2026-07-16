using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Mục tiêu bám theo")]
    public Transform target;
    
    [Header("Độ trễ mượt mà")]
    [Tooltip("Số càng lớn thì camera bám càng chặt, số càng nhỏ thì độ trễ càng nhiều")]
    public float smoothSpeed = 5f;

    [Header("Độ dời của Camera (Khoảng cách Z)")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Giới hạn di chuyển của Camera (Mép toa tàu)")]
    public bool useBounds = true;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Vị trí mà Camera mong muốn đi tới
        Vector3 desiredPosition = target.position + offset;

        // Tính toán vị trí trượt mượt mà
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Chặn giới hạn không cho Camera lọt ra khỏi toa tàu
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        // Cập nhật vị trí Camera
        transform.position = smoothedPosition;
    }
}

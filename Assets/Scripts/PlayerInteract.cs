using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Transform interactPoint;
    public float interactRadius = 1f;
    public LayerMask interactLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (interactPoint == null)
        {
            Debug.LogWarning("Chưa gắn interactPoint vào Player!");
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(interactPoint.position, interactRadius, interactLayer);

        if (hit != null)
        {
            Interactable interactable = hit.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
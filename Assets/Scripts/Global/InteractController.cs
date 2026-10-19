using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float range = 3f;
    [SerializeField] LayerMask mask = ~0;         
    [SerializeField] GameObject interactUI;        

    LeverActivate current;

    void Update()
    {
        LeverActivate found = null;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range, mask, QueryTriggerInteraction.Collide))
            hit.collider.TryGetComponent(out found);

        if (found != current)
        {
            current = found;
            interactUI.SetActive(current != null);
        }

        if (current != null && Keyboard.current.eKey.wasPressedThisFrame)
            current.Interact();
    }

    void OnDisable()
    {
        current = null;
        if (interactUI != null) interactUI.SetActive(false);
    }
}
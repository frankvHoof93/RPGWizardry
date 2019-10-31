using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    Image draggedImage;
    Vector3 mousePos;

    public GraphicRaycaster caster;
    public EventSystem eventSystem;

    private void Update()
    {
        // Click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            // Raycast and select

            PointerEventData data = new PointerEventData(eventSystem);
            data.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            caster.Raycast(data, results);

            if (results.Count > 0)
            {
                Debug.Log("Hit");
                draggedImage = results[0].gameObject.GetComponent<Image>();
                mousePos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButton(0) && draggedImage != null) // Held (Drag)
        {
            Debug.Log("Drag");
            // Move Image-Pos
            float deltaY = Input.mousePosition.y - mousePos.y;
            draggedImage.transform.Translate(0, deltaY, 0, Space.Self);
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Released
        {
            Debug.Log("Release");
            // Deselect
            draggedImage = null;
        }
    }
}

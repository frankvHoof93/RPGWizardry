using nl.SWEG.Willow.Utils.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace nl.SWEG.Willow.Research
{ 
    /// <summary>
    /// Handles User-Input for Research MiniGame
    /// </summary>
    public class ResearchInput : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// GraphicRayCaster for Canvas
        /// </summary>
        [SerializeField]
        [Tooltip("GraphicRayCaster for Canvas")]
        private GraphicRaycaster caster;
        /// <summary>
        /// EventSystem for Input
        /// </summary>
        [SerializeField]
        [Tooltip("EventSystem for Input")]
        private EventSystem eventSystem;
        /// <summary>
        /// Tag for bars moved by this Script
        /// </summary>
        [SerializeField]
        [TagSelector]
        [Tooltip("Tag for bars moved by this Script")]
        private string barTag;
        #endregion

        #region Private
        /// <summary>
        /// Image currently being dragged
        /// </summary>
        private Image draggedImage;
        /// <summary>
        /// Mouse-Position for last Frame
        /// </summary>
        private Vector3 mousePos;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Checks for Inputs
        /// </summary>
        private void Update()
        {
            MouseInputs();
        }

        /// <summary>
        /// Handles MouseInput
        /// </summary>
        private void MouseInputs()
        {
            // Click
            if (Input.GetMouseButtonDown(0))
            {
                // Raycast and select
                PointerEventData data = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };
                List<RaycastResult> results = new List<RaycastResult>();
                caster.Raycast(data, results);
                if (results.Count > 0 && results[0].gameObject.tag == barTag) // Found an object
                {
                    draggedImage = results[0].gameObject.GetComponent<Image>(); // Set as Dragged
                    mousePos = Input.mousePosition; // Store position
                }
            }
            else if (Input.GetMouseButton(0) && draggedImage != null) // Held (Drag)
            {
                // Move Image-Pos
                float deltaY = Input.mousePosition.y - mousePos.y;
                draggedImage.transform.Translate(0, deltaY, 0, Space.Self);
                // Clamp Position
                draggedImage.transform.localPosition = new Vector3(draggedImage.transform.localPosition.x, Mathf.Clamp(draggedImage.transform.localPosition.y, -45, 45), draggedImage.transform.localPosition.z);
                mousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0)) // Released
            {
                // Deselect
                draggedImage = null;
            }
        }
        #endregion
    }
}
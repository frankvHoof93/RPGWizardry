using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.Player.PlayerInput
{
    public class SpellCraftingInput : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// GraphicRayCaster for Canvas
        /// </summary>
        [SerializeField]
        private GraphicRaycaster caster;
        /// <summary>
        /// EventSystem for Input
        /// </summary>
        [SerializeField]
        private EventSystem eventSystem;
        #endregion

        #region Private
        /// <summary>
        /// Image being dragged
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
        /// Checks for Inputs (if Enabled)
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
                PointerEventData data = new PointerEventData(eventSystem);
                data.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                caster.Raycast(data, results);

                if (results.Count > 0)
                {
                    if(results[0].gameObject.tag == "Bar")
                    {
                        draggedImage = results[0].gameObject.GetComponent<Image>();
                        mousePos = Input.mousePosition;
                    }

                }
            }
            else if (Input.GetMouseButton(0) && draggedImage != null) // Held (Drag)
            {
                // Move Image-Pos
                float deltaY = Input.mousePosition.y - mousePos.y;
                draggedImage.transform.Translate(0, deltaY, 0, Space.Self);
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
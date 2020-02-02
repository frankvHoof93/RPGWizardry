using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Player.PlayerInput;
using UnityEngine;

namespace nl.SWEG.Willow.Player.Combat
{
    /// <summary>
    /// Handles Aiming for Player
    /// </summary>
    [RequireComponent(typeof(InputManager))]
    public class AimingManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Transform of the book's pivot
        /// </summary>
        [SerializeField]
        [Tooltip("Transform of the book's pivot")]
        private Transform bookPivot;
        /// <summary>
        /// Animator for the book
        /// </summary>
        [SerializeField]
        [Tooltip("Animator for the book")]
        private Animator bookAnimator;
        /// <summary>
        /// InputManager for Player
        /// </summary>
        private InputManager playerInput;
        #endregion

        #region Methods
        /// <summary>
        /// Grabs reference to InputManager
        /// </summary>
        private void Start()
        {
            playerInput = GetComponent<InputManager>();
        }

        /// <summary>
        /// Handles aiming based on Input
        /// </summary>
        private void Update()
        {
            PivotToMouse();
        }

        /// <summary>
        /// Rotates a pivot to point at the mouse, aiming at it
        /// Also changes the Z position of the book to rotate around the player
        /// </summary>
        private void PivotToMouse()
        {
            // If the player is allowed to move
            if (!GameManager.Instance.Paused)
            {
                // Get location to look at
                Vector3 lookPos = playerInput.State.AimDirection;
                float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
                // Rotate to look at mouse/controller direction
                bookPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // Change rotation parameter in animator, changing sprites
                bookAnimator.SetFloat("Rotation", bookPivot.rotation.z);
            }
        }
        #endregion
    }
}
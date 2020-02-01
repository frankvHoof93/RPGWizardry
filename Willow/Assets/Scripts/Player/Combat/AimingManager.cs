using nl.SWEG.Willow.Player.PlayerInput;
using UnityEngine;

namespace nl.SWEG.Willow.Player.Combat
{
    [RequireComponent(typeof(PlayerManager))]
    public class AimingManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Transform of the book's pivot
        /// </summary>
        [SerializeField]
        private Transform BookPivot;
        /// <summary>
        /// Manager for Player
        /// </summary>
        private PlayerManager player;
        /// <summary>
        /// Animator for the book
        /// </summary>
        [SerializeField]
        private Animator bookAnimator;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Grabs inputstate reference for aiming
        /// </summary>
        private void Start()
        {
            player = GetComponent<PlayerManager>();
        }

        /// <summary>
        /// Handles aiming based on Input
        /// </summary>
        private void Update()
        {
            PivotToMouse();
        }

        #endregion

        #region Private
        /// <summary>
        /// Rotates a pivot to point at the mouse, aiming at it
        /// Also changes the Z position of the book to rotate around the player
        /// </summary>
        private void PivotToMouse()
        {
            //If the player is allowed to move
            if (!GameManager.Instance.Paused)
            {
                //Get location to look at
                Vector3 lookPos = PlayerManager.Instance.InputManager.State.AimDirection;
                float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
                //Rotate to look at mouse/controller direction
                BookPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                //Change rotation parameter in animator, changing sprites
                bookAnimator.SetFloat("Rotation", BookPivot.rotation.z);

                //Change Z position, rotating around player appropriately
                if (BookPivot.rotation.z > 0)
                {
                    if (BookPivot.localPosition.z < 1)
                    {
                        BookPivot.localPosition = new Vector3(0, 0.5f, 1);
                    }
                }
                else if (BookPivot.rotation.z < 0)
                {
                    if (BookPivot.localPosition.z > -1)
                    {
                        BookPivot.localPosition = new Vector3(0, 0.5f, -1);
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}
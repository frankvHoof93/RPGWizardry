using nl.SWEG.RPGWizardry.PlayerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar.Combat
{
    [RequireComponent(typeof(InputManager))]
    public class AimingManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Transform of the book's pivot
        /// </summary>
        [SerializeField]
        private Transform BookPivot;
        /// <summary>
        /// InputManager for Aiming
        /// </summary>
        private InputManager inputManager;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Grabs inputmanager for aiming
        /// </summary>
        void Start()
        {
            inputManager = GetComponent<InputManager>();
        }

        /// <summary>
        /// Handles aiming based on Input
        /// </summary>
        void Update()
        {
            PivotToMouse();
        }

        #endregion

        #region Private
        /// <summary>
        /// Rotates a pivot to point at the mouse, aiming at it
        /// </summary>
        void PivotToMouse()
        {
            Vector3 lookPos = inputManager.AimingData;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            BookPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        #endregion
        #endregion
    }
}


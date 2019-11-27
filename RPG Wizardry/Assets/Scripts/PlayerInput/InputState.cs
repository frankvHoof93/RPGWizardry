using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.PlayerInput
{

    public class InputState : MonoBehaviour
    {
        #region Variables
        #region Public

        /// <summary>
        /// Outgoing movement values: X horizontal, Y vertical, Z depth (render order)
        /// </summary>
        public Vector3 MovementData;
        /// <summary>
        /// Outgoing movement values: essentially location vector 3 of look target
        /// </summary>
        public Vector3 AimingData;
        /// <summary>
        /// Outgoing button values: Cast1: left mouse / A button
        /// </summary>
        public bool Cast1;

        #endregion
        #endregion
    }
}



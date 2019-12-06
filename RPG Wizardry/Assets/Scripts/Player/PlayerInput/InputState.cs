using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.PlayerInput
{
    public struct InputState
    {
        #region Variables
        /// <summary>
        /// Outgoing movement values: X horizontal, Y vertical
        /// </summary>
        public Vector2 MovementData;
        /// <summary>
        /// Look-Direction
        /// </summary>
        public Vector2 AimingData;
        /// <summary>
        /// Outgoing button values: left mouse / A button
        /// </summary>
        public bool Cast1;
        #endregion
    }
}
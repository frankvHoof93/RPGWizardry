using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.PlayerInput
{
    public struct InputState
    {
        #region InnerTypes
        public enum SpellSelection : int
        {
            None = 0,
            Index1 = 1,
            Index2 = 2,
            Index3 = 3,
            Index4 = 4,
            SelectPrevious = 5,
            SelectNext = 6
        }
        #endregion

        #region Variables
        /// <summary>
        /// Outgoing movement values: X horizontal, Y vertical
        /// </summary>
        public Vector2 MovementDirection;
        /// <summary>
        /// Look-Direction
        /// </summary>
        public Vector2 AimDirection;
        /// <summary>
        /// Outgoing button values: left mouse / A button
        /// </summary>
        public bool Cast;
        /// <summary>
        /// Index of Cast Spell (Only for Controller)
        /// </summary>
        public int? CastIndex;
        /// <summary>
        /// Selection of Spell (Only for Mouse/Keyboard)
        /// </summary>
        public SpellSelection SelectSpell;
        #endregion
    }
}
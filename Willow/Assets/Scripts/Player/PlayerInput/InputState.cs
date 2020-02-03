using UnityEngine;

namespace nl.SWEG.Willow.Player.PlayerInput
{
    /// <summary>
    /// Holds Inputs from Players
    /// </summary>
    public struct InputState
    {
        #region InnerTypes
        /// <summary>
        /// Inputs for Spell-Selection
        /// </summary>
        public enum SpellSelection : byte
        {
            /// <summary>
            /// No Input
            /// </summary>
            None = 0,
            /// <summary>
            /// Select Index 1
            /// </summary>
            Index1 = 1,
            /// <summary>
            /// Select Index 2
            /// </summary>
            Index2 = 2,
            /// <summary>
            /// Select Index 3
            /// </summary>
            Index3 = 3,
            /// <summary>
            /// Select Index 4
            /// </summary>
            Index4 = 4,
            /// <summary>
            /// Select Previous Spell
            /// </summary>
            SelectPrevious = 5,
            /// <summary>
            /// Select Next Spell
            /// </summary>
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
using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Utils.Enums
{
    /// <summary>
    /// Direction in 2D-Space
    /// </summary>
    [Flags]
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }

    public static class DirectionExtensions
    {
        /// <summary>
        /// Returns Vector for Direction
        /// </summary>
        /// <param name="direction">Direction to get Vector for</param>
        /// <returns>Vector for Direction</returns>
        public static Vector2 GetVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector2.up;
                case Direction.Down:
                    return Vector2.down;
                case Direction.Left:
                    return Vector2.left;
                case Direction.Right:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
        }
    }
}
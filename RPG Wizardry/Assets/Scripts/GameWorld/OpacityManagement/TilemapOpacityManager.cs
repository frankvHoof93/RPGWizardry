using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class TilemapOpacityManager : OpacityManager
    {
        #region Variables
        /// <summary>
        /// The wall tilemaps.
        /// </summary>
        [Space]
        [SerializeField]
        private List<Tilemap> walls;
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Changes the alpha of all relevant walls.
        /// </summary>
        /// <param name="a">The alpha value the walls need to be changed to</param>
        protected override void ChangeAlpha(float a)
        {
            for (int i = 0; i < walls.Count; i++)
            {
                Tilemap map = walls[i];
                map.color = new Color(map.color.r, map.color.g, map.color.b, a);
            }
        }
        #endregion
        #endregion
    }
}
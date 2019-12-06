using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererOpacityManager : OpacityManager
{
        #region Variables
        /// <summary>
        /// The sprite renderer.
        /// </summary>
        [Space]
        private SpriteRenderer spriteRenderer;
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Changes the alpha of the sprite.
        /// </summary>
        /// <param name="a">The alpha value the SpriteRenderer needs to be changed to</param>
        protected override void ChangeAlpha(float a)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
        }
        #endregion
        #region Unity
        /// <summary>
        /// Gets the SpriteRenderer.
        /// </summary>
        private void Start()
        {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        #endregion
        #endregion
    }
}
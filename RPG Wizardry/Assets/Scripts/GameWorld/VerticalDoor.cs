using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class VerticalDoor : Door
    {
        #region Variables
        /// <summary>
        /// The opened door sprite.
        /// </summary>
        [SerializeField]
        private Sprite openSprite;

        /// <summary>
        /// The closed door sprite.
        /// </summary>
        [SerializeField]
        private Sprite closedSprite;

        /// <summary>
        /// The door sprite.
        /// </summary>
        private SpriteRenderer doorRenderer;
        #endregion

        #region Methods
        #region Public
        /// <summary>.
        /// Opens the door.
        /// </summary>
        public override void Open()
        {
            base.Open();
            doorRenderer.sprite = openSprite;
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public override void Close()
        {
            base.Close();
            doorRenderer.sprite = closedSprite;
        }
        #endregion
        #region Unity
        /// <summary>
        /// Gets the spriterendere of the door, and calls base.start.
        /// </summary>
        private void Start()
        {
            doorRenderer = GetComponent<SpriteRenderer>();
            base.Start();
        }
        #endregion
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The light that gets displayed when the door is opened.
        /// </summary>
        [SerializeField]
        private GameObject lights;

        /// <summary>
        /// The collider of the door.
        /// </summary>
        private Collider2D hitbox;
        #endregion

        #region Methods
        #region Public
        /// <summary>.
        /// Opens the door.
        /// </summary>
        public virtual void Open()
        {
            hitbox.enabled = false;
            lights.SetActive(true);
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public virtual void Close()
        {
            hitbox.enabled = true;
            lights.SetActive(false);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Gets the collider, and closes the door.
        /// </summary>
        protected void Start()
        {
            hitbox = GetComponent<Collider2D>();
            Close();
        }
        #endregion
        #endregion
    }
}
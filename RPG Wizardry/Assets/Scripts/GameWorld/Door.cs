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
        /// The room the door is leading to.
        /// </summary>
        public Room Room { get; private set; }

        /// <summary>
        /// The other side of the door, in a different room.
        /// </summary>
        [SerializeField]
        private Door destination;

        /// <summary>
        /// The light that gets displayed when the door is opened.
        /// </summary>
        [SerializeField]
        private GameObject lights;

        /// <summary>
        /// The collider of the door.
        /// </summary>
        [SerializeField]
        private Collider2D collider;
        #endregion

        #region Methods
        #region Public
        /// <summary>.
        /// Opens the door.
        /// </summary>
        public virtual void Open()
        {
            collider.enabled = false;
            lights.SetActive(true);
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public virtual void Close()
        {
            collider.enabled = true;
            lights.SetActive(false);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Gets the collider, and closes the door.
        /// </summary>
        protected virtual void Awake()
        {
            Room = GetComponentInParent<Room>();
        }

        /// <summary>
        /// Checks if the player is hitting the room switch trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            FloorManager.Instance.SwitchTo(destination);
        }
        #endregion
        #endregion
    }
}
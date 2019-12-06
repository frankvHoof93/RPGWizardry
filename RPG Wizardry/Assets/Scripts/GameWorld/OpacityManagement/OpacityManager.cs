using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class OpacityManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The filter that makes sure only the player is able to trigger the methods.
        /// </summary>
        [SerializeField]
        private ContactFilter2D contactFilter;

        /// <summary>
        /// The trigger that checks if the player enters it.
        /// </summary>
        private Collider2D collider;
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Uses Collider.OverlapCollider() to check if there are any Players or enemies in the collider.
        /// </summary>
        /// <returns>True if the collider is empty, false if it is not.</returns>
        private bool OverlapsWithEntity()
        {
            List<Collider2D> results = new List<Collider2D>();
            return collider.OverlapCollider(contactFilter, results) == 1;
        }

        /// <summary>
        /// Changed the alpha of all relevant objects.
        /// </summary>
        /// <param name="a">The alpha value the objects need to be changed to</param>
        protected abstract void ChangeAlpha(float a);
        #endregion

        #region Unity
        /// <summary>
        /// Gets the collider.
        /// </summary>
        protected void Start()
        {
            collider = GetComponent<Collider2D>();
        }

        /// <summary>
        /// Checks if the thing entering the trigger is a player, and if it's fhe first thing to enter it, makes the walls transparent.
        /// </summary>
        /// <param name="collision">The thing entering the collider.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Check if the object is only hit by the Player.
            if (OverlapsWithEntity())
            {
                //Set the alpha of the renderer to half.
                ChangeAlpha(0.5f);
            }
        }

        /// <summary>
        /// Check if it's the last thing leaving the trigger, and if so, makes walls opague.
        /// </summary>
        /// <param name="collision">the thing leaving the collider.</param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            //If there is nothing else in the trigger.
            if (!OverlapsWithEntity())
            {
                //Set the alpha of the renderer to full.
                ChangeAlpha(1);
            }
        }
        #endregion
        #endregion
    }
}
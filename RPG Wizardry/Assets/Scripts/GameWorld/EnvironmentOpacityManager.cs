using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class EnvironmentOpacityManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The filter that makes sure only the player is able to trigger the methods.
        /// </summary>
        [SerializeField]
        private ContactFilter2D ContactFilter;

        /// <summary>
        /// The trigger that checks if the player enters it.
        /// </summary>
        private Collider2D Collider;

        /// <summary>
        /// The spriterenderer of the object.
        /// </summary>
        private SpriteRenderer Renderer;
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Uses Collider.OverlapCollider() to check if there are any Players or enemies in the collider.
        /// </summary>
        /// <returns>True if the collider is empty, false if it is not.</returns>
        private int GetOverlapData()
        {
            List<Collider2D> results = new List<Collider2D>();
            int output = Collider.OverlapCollider(ContactFilter, results);
            return output;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Gets the collider.
        /// </summary>
        void Start()
        {
            Collider = GetComponent<Collider2D>();
            Renderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Checks if the thing entering the trigger is a player, and if it's fhe first thing to enter it, makes the walls transparent.
        /// </summary>
        /// <param name="collision">The thing entering the collider.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Check if the object is only hit by the Player.
            if (collision.gameObject.layer == 11 && GetOverlapData() == 1)
            {
                //Set the alpha of the renderer to half.
                Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 0.5f);

            }
        }

        /// <summary>
        /// Check if it's the last thing leaving the trigger, and if so, makes walls opague.
        /// </summary>
        /// <param name="collision">the thing leaving the collider.</param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            //If there is nothing else in the trigger.
            if (GetOverlapData() == 0)
            {
                //Set the alpha of the renderer to full.
                Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, 1f);
            }
        }
        #endregion
        #endregion
    }
}
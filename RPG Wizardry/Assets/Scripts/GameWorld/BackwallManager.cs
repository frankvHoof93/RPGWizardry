using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public class BackwallManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The filter that makes sure only the player is able to trigger the methods.
        /// </summary>
        [SerializeField]
        private ContactFilter2D ContactFilter;

        /// <summary>
        /// The wall tilemaps.
        /// </summary>
        [Space]
        [SerializeField]
        private List<Tilemap> Walls;

        /// <summary>
        /// The trigger that checks if the player enters it.
        /// </summary>
        private Collider2D Collider;
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
                //Set the alpha of each tilemap to half.
                foreach (Tilemap s in Walls)
                {
                    s.color = new Color(s.color.r, s.color.g, s.color.b, 0.5f);
                }
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
                //Set the alpha of each tilemap to full.
                foreach (Tilemap s in Walls)
                {
                    s.color = new Color(s.color.r, s.color.g, s.color.b, 1f);
                }
            }
        }
        #endregion
        #endregion
    }
}
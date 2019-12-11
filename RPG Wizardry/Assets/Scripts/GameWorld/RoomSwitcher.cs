using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nl.SWEG.RPGWizardry.Player;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class RoomSwitcher : MonoBehaviour
    {
        #region Variables     
        //camera for fading

        //transform spawnposition
        //original room to disable?
        //new room to activate
        //player to move
        //private PlayerManager
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Switches the player 
        /// </summary>
        private void SwitchRoom()
        {
            //Lock player controls or game

            //Fade camera to black

            //Enable new room

            //Move player to new room

            //Disable old room

            //Unlock player controls or game

            //Fade player back in

            //activate enemies in new room

        }
        #endregion
        #region Unity
        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            if (PlayerManager.Exists)
            {

            }
        }

        /// <summary>
        /// Checks if the player is hitting the room switch trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            SwitchRoom();
        }
        #endregion
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nl.SWEG.RPGWizardry.Player;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomSwitcher : MonoBehaviour
    {
        #region Variables     
        public GameObject Room;
        public GameObject TargetRoom;
        public Transform TargetSpawn;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Checks if the player is hitting the room switch trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            CameraManager.Instance.SwitchRoom(Room, TargetRoom, TargetSpawn);
        }
        #endregion
        #endregion
    }
}
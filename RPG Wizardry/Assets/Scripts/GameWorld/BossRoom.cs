using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class BossRoom : Room
    {
        /// <summary>
        /// Door which ends Floor
        /// </summary>
        [SerializeField]
        [Tooltip("Door which ends Floor")]
        private GameObject endDoor;

        #region Methods
        /// <summary>
        /// Enables Final Door if Room has been Cleared
        /// </summary>
        protected override void CheckRoomClear()
        {
            base.CheckRoomClear();
            if (Cleared)
                OpenFinalDoor();
        }
        /// <summary>
        /// Opens door to end of Floor
        /// </summary>
        private void OpenFinalDoor()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
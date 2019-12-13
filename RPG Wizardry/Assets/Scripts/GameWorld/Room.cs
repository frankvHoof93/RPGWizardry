using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class Room : MonoBehaviour
    {
        public bool Cleared;

        /// <summary>
        /// All doors in the room.
        /// </summary>
        [SerializeField]
        private Door[] doors;

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Close all the doors in the room.
        /// </summary>
        public void CloseDoors()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Close();
            }
        }

        /// <summary>
        /// Opens all the doors in the room.
        /// </summary>
        public void OpenDoors()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Open();
            }
        }
    }
}
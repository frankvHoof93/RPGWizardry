using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nl.SWEG.RPGWizardry.Entities.Enemies;

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

        [Space]
        public GameObject EnemyHolder;

        private void Start()
        {
            foreach (AEnemy enemy in EnemyHolder.GetComponentsInChildren<AEnemy>(true))
            {
                enemy.Killed += CheckRoomClear;
                print("Enemy");
            }
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            if (!Cleared)
            {
                CloseDoors();
            }
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

        /// <summary>
        /// Checks if the room still has enemies. if it doesn't, the doors open.
        /// </summary>
        private void CheckRoomClear()
        {
            print(EnemyHolder.transform.childCount);
            if (EnemyHolder.transform.childCount == 0)
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    OpenDoors();
                }
            }
        }
    }
}
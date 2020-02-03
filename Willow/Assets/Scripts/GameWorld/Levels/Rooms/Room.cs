using nl.SWEG.Willow.Entities.Enemies;
using System;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld.Levels.Rooms
{
    /// <summary>
    /// A Room is an environment within a Floor. When a Player enters a Room containing Enemies, its Doors are closed until all Enemies have been defeated
    /// </summary>
    public class Room : MonoBehaviour
    {
        #region Inner Types
        /// <summary>
        /// Delegate for RoomClear-Event
        /// </summary>
        public delegate void RoomClear();

        /// <summary>
        /// Template for Spawning Enemies in a Room
        /// </summary>
        [Serializable]
        public struct SpawnTemplate
        {
            /// <summary>
            /// Prefab for Enemy to Spawn
            /// </summary>
            [Tooltip("Prefab for Enemy to Spawn")]
            public GameObject enemyPrefab;
            /// <summary>
            /// (Relative) Position to Spawn Enemy at
            /// </summary>
            [Tooltip("(Relative) Position to Spawn Enemy at")]
            public Vector2 spawnPosition;
        }
        #endregion

        #region Variables
        #region Public
        /// <summary>
        /// Whether the Room has been Cleared (No more Enemies)
        /// </summary>
        public bool Cleared => EnemyHolder.transform.childCount == 0;
        /// <summary>
        /// Roomcleared event
        /// </summary>
        public static event RoomClear clearedRoom; // TODOCLEAN:
        #endregion

        #region Editor
        /// <summary>
        /// All doors in the room.
        /// </summary>
        [SerializeField]
        [Tooltip("All doors in the room.")]
        private Door[] doors;
        /// <summary>
        /// Transform-Parent for Enemies in Room
        /// </summary>
        [Space]
        [SerializeField]
        [Tooltip("Transform-Parent for Enemies in Room")]
        private GameObject EnemyHolder;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Opens/Closes Doors based on whether there are Enemies in the Room
        /// </summary>
        public void CheckDoors()
        {
            CheckRoomClear();
        }
        #endregion

        #region Internal
        /// <summary>
        /// Initializes Room
        /// </summary>
        internal void Enable()
        {
            gameObject.SetActive(true);
            if (!Cleared)
                CloseDoors();
        }

        /// <summary>
        /// Unloads Room
        /// </summary>
        internal void Disable()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Closes all the Doors in the room
        /// </summary>
        internal void CloseDoors()
        {
            for (int i = 0; i < doors.Length; i++)
                doors[i].Close();
        }

        /// <summary>
        /// Opens all the Doors in the room
        /// </summary>
        internal void OpenDoors()
        {
            for (int i = 0; i < doors.Length; i++)
                doors[i].Open();
        }
        #endregion

        #region Unity
        /// <summary>
        /// Adds Event-Listeners to Enemies
        /// </summary>
        private void Awake()
        {
            AEnemy[] enemies = EnemyHolder.GetComponentsInChildren<AEnemy>(true);
            if (enemies.Length > 0)
            {
                foreach (AEnemy enemy in enemies)
                    enemy.AddDeathListener(CheckRoomClear); // TODOCLEAN: Spawn Enemies
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Checks if the room still has enemies. if it doesn't, the doors open.
        /// </summary>
        protected virtual void CheckRoomClear()
        {
            if (EnemyHolder == null)
                return;
            if (Cleared)
            {
                clearedRoom?.Invoke(); //Runs event when room is cleared
                OpenDoors();
            }
        }
        #endregion
        #endregion
    }
}
using nl.SWEG.Willow.Entities.Enemies;
using System;
using System.Collections.Generic;
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
            public GameObject EnemyPrefab;
            /// <summary>
            /// (Relative) Position to Spawn Enemy at
            /// </summary>
            [Tooltip("(Relative) Position to Spawn Enemy at")]
            public Vector2 SpawnPosition;
            /// <summary>
            /// Rotation (around up) to Spawn Enemy at
            /// </summary>
            [Tooltip("Rotation (around up) to Spawn Enemy at")]
            public float SpawnRotation;
        }
        #endregion

        #region Variables
        #region Public
        /// <summary>
        /// Whether the Room has been Cleared (No more Enemies)
        /// </summary>
        public bool Cleared => enemies.Count == 0;
        /// <summary>
        /// List of Enemies in Room
        /// </summary>
        public IReadOnlyList<AEnemy> Enemies => enemies.AsReadOnly();
        #endregion

        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
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
        private GameObject enemyHolder;
        /// <summary>
        /// Enemies to Spawn in Room
        /// </summary>
        [SerializeField]
        [Tooltip("Enemies to Spawn in Room")]
        private SpawnTemplate[] enemiesInRoom;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Whether this Room has spawned its enemies (prevents duplicate spawning when re-entering Room)
        /// </summary>
        private bool hasSpawnedEnemies;
        /// <summary>
        /// RoomCleared event
        /// </summary>
        private event RoomClear clearedRoom;
        /// <summary>
        /// Enemies in Room (Instances)
        /// </summary>
        private readonly List<AEnemy> enemies = new List<AEnemy>();
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Adds Listener to RoomClear-Event for this Room
        /// </summary>
        /// <param name="listener">Listener to add</param>
        public void AddRoomClearListener(RoomClear listener)
        {
            clearedRoom += listener;
        }

        /// <summary>
        /// Removes Listener from RoomClear-Event for this Room
        /// </summary>
        /// <param name="listener">Listener to remove</param>
        public void RemoveRoomClearListener(RoomClear listener)
        {
            clearedRoom -= listener;
        }
        #endregion

        #region Internal
        /// <summary>
        /// Initializes Room
        /// </summary>
        internal void Enable()
        {
            gameObject.SetActive(true);
            if (!hasSpawnedEnemies)
                SpawnEnemies();
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

        #region Private
        /// <summary>
        /// Checks if the room still has enemies. if it doesn't, the doors open.
        /// </summary>
        protected virtual void CheckRoomClear(GameObject deadEnemy)
        {
            enemies.Remove(deadEnemy.GetComponent<AEnemy>());
            if (GameManager.Exists && GameManager.Instance.State != GameManager.GameState.GameOver && Cleared)
            {
                clearedRoom?.Invoke(); //Runs event when room is cleared
                OpenDoors();
            }
        }

        /// <summary>
        /// Spawns Enemies in Room
        /// </summary>
        private void SpawnEnemies()
        {
            if (hasSpawnedEnemies)
                return;
            for (int i = 0; i < enemiesInRoom.Length; i++)
            {
                SpawnTemplate template = enemiesInRoom[i];
                GameObject enemy = Instantiate(template.EnemyPrefab);
                enemy.transform.SetParent(enemyHolder.transform);
                enemy.transform.localPosition = template.SpawnPosition;
                enemy.transform.rotation = Quaternion.Euler(0, 0, template.SpawnRotation);
                AEnemy script = enemy.GetComponent<AEnemy>();
                enemies.Add(script);
                script.AddDeathListener(CheckRoomClear);
            }
            hasSpawnedEnemies = true;
        }
        #endregion
        #endregion
    }
}
using System;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "")]
    public class RoomData : ScriptableObject
    {
        #region InnerTypes
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
        /// Prefab for Room (Floor, Walls, etc.)
        /// </summary>
        public Room RoomPrefab => roomPrefab;
        /// <summary>
        /// Templates for Spawning Enemies
        /// </summary>
        public SpawnTemplate[] Spawns => spawns;
        #endregion

        #region Editor
        /// <summary>
        /// Prefab for Room (Floor, Walls, etc.)
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab for Room (Floor, Walls, etc.)")]
        private Room roomPrefab;
        /// <summary>
        /// Templates for Spawning Enemies
        /// </summary>
        [SerializeField]
        [Tooltip("Templates for Spawning Enemies")]
        private SpawnTemplate[] spawns;
        #endregion
        #endregion
    }
}
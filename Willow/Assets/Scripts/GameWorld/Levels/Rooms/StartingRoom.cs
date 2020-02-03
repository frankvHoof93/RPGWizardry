using UnityEngine;

namespace nl.SWEG.Willow.GameWorld.Levels.Rooms
{
    /// <summary>
    /// Starting Room within a Floor. Hold SpawnPoint for that Floor
    /// </summary>
    public class StartingRoom : Room
    {
        /// <summary>
        /// SpawnPoint in Room
        /// </summary>
        public Transform SpawnPoint => spawnPoint;
        /// <summary>
        /// SpawnPoint in Room
        /// </summary>
        [SerializeField]
        [Tooltip("SpawnPoint in Room")]
        private Transform spawnPoint;

    }
}
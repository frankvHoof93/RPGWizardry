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
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// SpawnPoint in Room
        /// </summary>
        [SerializeField]
        [Tooltip("SpawnPoint in Room")]
        private Transform spawnPoint;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
    }
}
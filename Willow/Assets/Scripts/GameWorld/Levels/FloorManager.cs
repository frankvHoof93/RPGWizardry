using nl.SWEG.Willow.GameWorld.Levels.Rooms;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Utils.Behaviours;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld.Levels
{
    /// <summary>
    /// Handles Floors within the GameWorld
    /// <para>
    /// A Floor is a single 'Level' within the GameWorld, and consists of multiple Rooms
    /// </para>
    /// </summary>
    public class FloorManager : SingletonBehaviour<FloorManager>
    {
        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// All Rooms on the Floor
        /// </summary>
        [SerializeField]
        [Tooltip("All Rooms on the Floor")]
        private Room[] rooms;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables

        /// <summary>
        /// Currently Loaded Room
        /// </summary>
        private Room activeRoom;
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Starts the coroutine that switches from one Room to another
        /// </summary>
        /// <param name="destination">Target-Door in new Room</param>
        public void SwitchTo(Door destination)
        {
            StartCoroutine(SwitchRoom(destination));
        }

        /// <summary>
        /// Returns SpawnPoint for current Floor
        /// </summary>
        /// <returns>WorldPosition for SpawnPoint, or Vector3.Zero if no Point could be found</returns>
        public Vector3 GetSpawnPoint()
        {
            return rooms.OfType<StartingRoom>().First()?.SpawnPoint.position ?? Vector3.zero;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Loads first Room
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < rooms.Length; i++)
                rooms[i].Disable();
            activeRoom = rooms[0];
            activeRoom.Enable();
        }
        #endregion

        #region Private
        /// <summary>
        /// Moves the player between 2 Rooms, and handles room visibility accordingly.
        /// </summary>
        /// <param name="destination">Target-Door in new Room</param>
        private IEnumerator SwitchRoom(Door destination)
        {
            // Make sure the game is paused
            if (!GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();
            // Fade the screen out
            CameraManager.instance.Fade(1, 0);
            while (CameraManager.instance.Fading)
                yield return null;
            // Disable the previous room
            activeRoom?.Disable();
            // Clear Loot from old Room
            LootSpawner.Instance.ClearLoot();
            // Move the player to new room
            PlayerManager.Instance.transform.position = destination.Spawn.position;
            // Enable the new room
            activeRoom = destination.Room;
            activeRoom.Enable();
            // Move Camera to Player
            CameraManager.Instance.transform.position = PlayerManager.Instance.transform.position;
            // Fade the screen back in
            CameraManager.instance.Fade(0, 1);
            while (CameraManager.instance.Fading)
                yield return null;
            // Make sure the player can move again
            if (GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();
            // Close doors in new Room if necessary
            activeRoom.CheckDoors();
        }
        #endregion
        #endregion
    }
}
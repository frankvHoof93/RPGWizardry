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
        #region InnerTypes
        /// <summary>
        /// Delegate for Event when a Room is Loaded/Unloaded
        /// </summary>
        /// <param name="room">Room that was Loaded/Unloaded</param>
        public delegate void OnRoomLoad(Room room);
        #endregion

        #region Variables
        /// <summary>
        /// Currently Loaded Room
        /// </summary>
        public Room CurrentRoom { get; private set; }

        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// All Rooms on the Floor
        /// </summary>
        [SerializeField]
        [Tooltip("All Rooms on the Floor")]
        private Room[] rooms;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables

        /// <summary>
        /// Event fired when Room is Loaded. Event is fired AFTER Room is Enabled
        /// </summary>
        private event OnRoomLoad onRoomLoad;
        /// <summary>
        /// Event fired when Room is Unloaded. Event is fired BEFORE Room is Disabled
        /// </summary>
        private event OnRoomLoad onRoomUnload;
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

        #region EventListeners
        /// <summary>
        /// Adds Listener to RoomLoad-Event
        /// </summary>
        /// <param name="listener">Listener to add</param>
        public void AddRoomLoadListener(OnRoomLoad listener)
        {
            onRoomLoad += listener;
        }

        /// <summary>
        /// Removes Listener from RoomLoad-Event
        /// </summary>
        /// <param name="listener">Listener to remove</param>
        public void RemoveRoomLoadListener(OnRoomLoad listener)
        {
            onRoomLoad -= listener;
        }

        /// <summary>
        /// Adds Listener to RoomUnload-Event
        /// </summary>
        /// <param name="listener">Listener to add</param>
        public void AddRoomUnloadListener(OnRoomLoad listener)
        {
            onRoomUnload += listener;
        }

        /// <summary>
        /// Removes Listener from RoomUnload-Event
        /// </summary>
        /// <param name="listener">Listener to remove</param>
        public void RemoveRoomUnloadListener(OnRoomLoad listener)
        {
            onRoomUnload -= listener;
        }
        #endregion
        #endregion

        #region Unity
        /// <summary>
        /// Disables all Rooms in Floor
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < rooms.Length; i++)
                rooms[i].Disable();
        }

        /// <summary>
        /// Enables first Room
        /// </summary>
        private void Start()
        {
            CurrentRoom = rooms[0];
            CurrentRoom.Enable();
            onRoomLoad?.Invoke(CurrentRoom);
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
            GameManager.Instance.PauseGame();
            // Fade the screen out
            CameraManager.instance.Fade(1, 0);
            while (CameraManager.instance.Fading)
                yield return null;
            // Disable the previous room
            onRoomUnload?.Invoke(CurrentRoom);
            CurrentRoom?.Disable();
            // Clear Loot from old Room
            LootSpawner.Instance.ClearLoot();
            // Move the player to new room
            PlayerManager.Instance.transform.position = destination.Spawn.position;
            // Enable the new room
            CurrentRoom = destination.Room;
            CurrentRoom.Enable();
            onRoomLoad?.Invoke(CurrentRoom);
            // Move Camera to Player
            CameraManager.Instance.transform.position = PlayerManager.Instance.transform.position;
            // Fade the screen back in
            CameraManager.instance.Fade(0, 1);
            while (CameraManager.instance.Fading)
                yield return null;
            // Make sure the player can move again
            GameManager.Instance.ResumeGame();
        }
        #endregion
        #endregion
    }
}
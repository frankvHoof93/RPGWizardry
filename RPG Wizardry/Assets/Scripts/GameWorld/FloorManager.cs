using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using nl.SWEG.RPGWizardry.UI.GameUI;
using System.Collections;
using UnityEngine;
using static nl.SWEG.RPGWizardry.GameWorld.RoomData;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class FloorManager : SingletonBehaviour<FloorManager>
    {
        #region Variables
        /// <summary>
        /// Rooms in Current Floor
        /// </summary>
        private RoomData[] floorRooms;

        /// <summary>
        /// A list of all rooms on the floor.
        /// </summary>
        [SerializeField]
        private Room[] rooms;

        /// <summary>
        /// Currently Loaded Room
        /// </summary>
        [HideInInspector]
        public Room activeRoom;
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Starts the coroutine that switches from one room to the other.
        /// </summary>
        /// <param name="destination">The door in the target room.</param>
        public void SwitchTo(Door destination)
        {
            StartCoroutine(switchRoom(destination));
        }

        /// <summary>
        /// Loads Floor, spawning Objects & Player
        /// </summary>
        public void LoadFloor()
        {
            if (activeRoom != null)
                activeRoom.Disable();
            CameraManager.instance.Fade(0, 1);
            activeRoom = rooms[0];
            activeRoom.Enable();
            GameManager.Instance.SpawnPlayer(activeRoom.GetPlayerSpawn());
        }

        /// <summary>
        /// UNUSED
        /// Loads Room by Index. Unloads current room, and spawns Enemies for Room as well
        /// </summary>
        /// <param name="index">Index for Room to Spawn</param>
        public void LoadRoom(uint index)
        {
            RoomData data = floorRooms[index];
            GameObject parent = new GameObject("SpawnedObjects");
            parent.transform.parent = activeRoom.transform;
            parent.transform.position = Vector3.zero;
            for (int i = 0; i < data.Spawns.Length; i++)
            {
                SpawnTemplate template = data.Spawns[i];
                GameObject enemy = Instantiate(template.enemyPrefab);
                enemy.transform.parent = parent.transform;
                enemy.transform.localPosition = template.spawnPosition;
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// moves the player between 2 rooms, and handles room visibility accordingly.
        /// </summary>
        /// <param name="previous">The room the player is currently in.</param>
        /// <param name="next">The room the player needs to go.</param>
        /// <param name="spawn">the place where the player needs to end up.</param>
        /// <returns></returns>
        private IEnumerator switchRoom(Door destination)
        {
            //Make sure the game is paused
            if (!GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();

            //Fade the screen out
            CameraManager.instance.Fade(1, 0);
            while (CameraManager.instance.Fading)
            {
                yield return null;
            }

            //Disable the old room
            activeRoom.Disable();

            // Clear Loot from old Room
            LootSpawner.Instance.ClearLoot();

            //Move the player to new room
            PlayerManager.Instance.transform.position = destination.Spawn.position;

            //Enable the new room
            activeRoom = destination.Room;
            activeRoom.Enable();

            // Move Camera to Player
            CameraMover.Instance.transform.position = PlayerManager.Instance.transform.position;

            //Fade the screen back in
            CameraManager.instance.Fade(0, 1);
            while (CameraManager.instance.Fading)
            {
                yield return null;
            }

            //Make sure the player can move again
            if (GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();

            //Activate new room
            if (!activeRoom.Cleared)
            {
                yield return new WaitForSeconds(0.25f);
                activeRoom.CloseDoors();
            }
            else
            {
                activeRoom.OpenDoors();
            }
        }
        #endregion

        #region Unity
        /// <summary>
        /// Loads first Room
        /// </summary>
        protected override void Awake()
        {
            for (int i = 0; i < rooms.Length; i++)
                rooms[i].Disable();
            base.Awake();
        }
        #endregion
        #endregion
    }
}
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System;
using UnityEngine;
using System.Collections;
using static nl.SWEG.RPGWizardry.GameWorld.RoomData;
using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Player;
using UnityEngine.Events;

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
        [Space]

        /// <summary>
        /// Currently Loaded Room
        /// </summary>
        private Room activeRoom;
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
            activeRoom = rooms[0];
            activeRoom.Enable();
            GameManager.Instance.SpawnPlayer(activeRoom.GetPlayerSpawn());
            StartCoroutine(startFade());
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
            //Make sure the player can't move
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
            
            //Move the player to new room
            if (PlayerManager.Exists)
                PlayerManager.Instance.transform.position = destination.transform.position;


            //Enable the new room
            destination.Room.Enable();
            activeRoom = destination.Room;

            //Fade the screen back in
            CameraManager.instance.Fade(0, 1);
            while (CameraManager.instance.Fading)
            {
                yield return null;
            }

            //Make sure the player can move again
            if (GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();


            //Activate enemies in new room
            if (!activeRoom.Cleared)
            {
                //Spawn enemies

                //close doors
                yield return new WaitForSeconds(0.25f);
                activeRoom.CloseDoors();
            }
            else
            {
                //open doors
                activeRoom.OpenDoors();
            }
        }

        private IEnumerator startFade()
        {
            yield return new WaitForSeconds(0.25f);

            //fade camera in
            CameraManager.instance.Fade(0, 1);
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
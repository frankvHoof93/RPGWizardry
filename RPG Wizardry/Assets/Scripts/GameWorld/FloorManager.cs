using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System;
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
        [SerializeField]
        private RoomData[] floorRooms;

        /// <summary>
        /// Currently Loaded Room
        /// </summary>
        private GameObject room;
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Loads Room by Index. Unloads current room, and spawns Enemies for Room as well
        /// </summary>
        /// <param name="index">Index for Room to Spawn</param>
        public void LoadRoom(uint index)
        {
            if (index >= floorRooms.Length)
                throw new ArgumentException("Index out of Range", "index");
            if (room != null)
                UnloadRoom();
            RoomData data = floorRooms[index];
            room = Instantiate(data.RoomPrefab);
            room.transform.parent = transform;
            room.transform.position = Vector3.zero;
            GameObject parent = new GameObject("SpawnedObjects");
            parent.transform.parent = room.transform;
            parent.transform.position = Vector3.zero;
            for (int i = 0; i < data.Spawns.Length; i++)
            {
                SpawnTemplate template = data.Spawns[i];
                GameObject enemy = Instantiate(template.enemyPrefab);
                enemy.transform.parent = parent.transform;
                enemy.transform.localPosition = template.spawnPosition;
            }
        }
        /// <summary>
        /// Unloads currently loaded Room
        /// </summary>
        public void UnloadRoom()
        {
            if (room != null)
                Destroy(room);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Loads first Room
        /// </summary>
        private void Start()
        {
            LoadRoom(0);
        }
        #endregion
        #endregion
    }
}
using nl.SWEG.RPGWizardry.Entities.Enemies;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class Room : MonoBehaviour
    {
        #region Variables
        #region Public
        /// <summary>
        /// Whether the Room has been Cleared (No more Enemies)
        /// </summary>
        public bool Cleared { get; private set; }
        #endregion

        #region Editor
        /// <summary>
        /// All doors in the room.
        /// </summary>
        [SerializeField]
        private Door[] doors;
        /// <summary>
        /// Parent for Enemies in Room
        /// </summary>
        [SerializeField]
        private GameObject EnemyHolder;
        /// <summary>
        /// SpawnPosition for Player
        /// </summary>
        [SerializeField]
        private Transform playerSpawnPos;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Gets Player-SpawnPosition for Room
        /// </summary>
        /// <returns>SpawnPosition for Player (WorldSpace)</returns>
        public Vector3 GetPlayerSpawn()
        {
            return playerSpawnPos.position;
        }
        #endregion

        #region Internal
        /// <summary>
        /// Enables Objects in Room
        /// </summary>
        internal void Enable()
        {
            gameObject.SetActive(true);
            if (!Cleared)
            {
                CloseDoors();
            }
        }
        /// <summary>
        /// Disables Objects in Room
        /// </summary>
        internal void Disable()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Closes all the doors in the room
        /// </summary>
        internal void CloseDoors()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Close();
            }
        }
        /// <summary>
        /// Opens all the doors in the room
        /// </summary>
        internal void OpenDoors()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].Open();
            }
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
                    enemy.Killed += CheckRoomClear;
            }
            else
                Cleared = true;
        }
        #endregion

        #region Private
        /// <summary>
        /// Checks if the room still has enemies. if it doesn't, the doors open.
        /// </summary>
        private void CheckRoomClear()
        {
            if (EnemyHolder == null)
                return;
            if (EnemyHolder.transform.childCount == 0)
            {
                Cleared = true;
                OpenDoors();
            }
        }
        #endregion
        #endregion
    }
}
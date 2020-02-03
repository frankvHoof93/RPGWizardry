using nl.SWEG.Willow.Entities.Collectables;
using nl.SWEG.Willow.Sorcery;
using nl.SWEG.Willow.Sorcery.Spells;
using nl.SWEG.Willow.Utils.Behaviours;
using System;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld
{
    /// <summary>
    /// Spawns Loot within the GameWorld
    /// </summary>
    public class LootSpawner : SingletonBehaviour<LootSpawner>
    {
        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Prefabs in order of Collectables-Enum
        /// </summary>
        [SerializeField]
        [Tooltip("Prefabs in order of Collectables-Enum")]
        private GameObject[] lootPrefabs = new GameObject[4];
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        /// <summary>
        /// Holds Loot, so it can be destroyed when a new Room is loaded
        /// </summary>
        private Transform lootHolder;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Creates LootHolder-Child
        /// </summary>
        private void Start()
        {
            lootHolder = new GameObject("LootHolder").transform;
            lootHolder.SetParent(transform, true);
        }
        #endregion

        #region Public
        /// <summary>
        /// Clears all Loot that is currently in the Scene
        /// </summary>
        public void ClearLoot()
        {
            for (int i = 0; i < lootHolder.childCount; i++)
                Destroy(lootHolder.GetChild(i).gameObject);
        }

        /// <summary>
        /// Spawns Loot
        /// </summary>
        /// <param name="loot">Loot to spawn</param>
        /// <param name="position">Position to spawn at</param>
        /// <param name="amount">Amount to set (for DustPile)</param>
        public void SpawnLoot(Collectables loot, Vector3 position, uint amount = 1)
        {
            GameObject spawnedObject = Instantiate(lootPrefabs[(int)loot]);
            spawnedObject.transform.position = position;
            spawnedObject.transform.SetParent(lootHolder, true);
            switch (loot)
            {
                case Collectables.Dust:
                    spawnedObject.GetComponent<DustPile>().SetAmount(amount);
                    break;
                case Collectables.Gold:
                    // TODO:
                    break;
                case Collectables.Page:
                    throw new ArgumentException("Call SpawnPage instead");
                case Collectables.Potion: 
                    // No Implementation, as there is no setting of amount for Potions
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Spawns a PageObject
        /// </summary>
        /// <param name="position">Position to Spawn at (WorldSpace)</param>
        /// <param name="spell">Spell for Page</param>
        public void SpawnPage(Vector3 position, SpellData spell)
        {
            GameObject spawnedObject = Instantiate(lootPrefabs[(int)Collectables.Page]);
            spawnedObject.GetComponent<PageObject>().Page = new SpellPage(spell);
            spawnedObject.transform.position = position;
            spawnedObject.transform.SetParent(lootHolder, true);
        }
        #endregion
        #endregion
    }
}

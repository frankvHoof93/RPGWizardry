using nl.SWEG.RPGWizardry.Entities.Collectables;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class LootSpawner : SingletonBehaviour<LootSpawner>
    {
        #region Variables
        /// <summary>
        /// Prefabs in order of Collectables enum
        /// </summary>
        [SerializeField]
        private GameObject[] lootPrefabs = new GameObject[4];
        /// <summary>
        /// Holds Loot, so it can be destroyed when a new Room is loaded
        /// </summary>
        private Transform lootHolder;
        #region

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
                    break;
                case Collectables.Page:
                    throw new ArgumentException("Call SpawnPage instead");
                case Collectables.Potion:
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
            spawnedObject.GetComponent<PageObject>().Page = new Sorcery.SpellPage(spell);
            spawnedObject.transform.position = position;
            spawnedObject.transform.SetParent(lootHolder, true);
        }
        #endregion
        #endregion
    }
}
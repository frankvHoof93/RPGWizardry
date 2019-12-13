using nl.SWEG.RPGWizardry.Entities.Collectables;
using nl.SWEG.RPGWizardry.Sorcery.Spells;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class LootSpawner : SingletonBehaviour<LootSpawner>
    {
        /// <summary>
        /// Prefabs in order of Collectables enum
        /// </summary>
        [SerializeField]
        private GameObject[] lootPrefabs = new GameObject[4];

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

        public void SpawnPage(Vector3 position, SpellData spell)
        {
            GameObject spawnedObject = Instantiate(lootPrefabs[(int)Collectables.Page]);
            spawnedObject.GetComponent<PageObject>().Page = new Sorcery.SpellPage(spell);
            spawnedObject.transform.position = position;
        }
    }
}

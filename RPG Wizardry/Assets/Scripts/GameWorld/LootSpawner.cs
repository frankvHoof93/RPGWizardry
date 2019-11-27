using nl.SWEG.RPGWizardry.Entities.Collectables;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
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

        // TODO: Use SingletonBehaviour
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
                    break;
                case Collectables.Potion:
                    break;
                default:
                    break;
            }
        }
    }
}

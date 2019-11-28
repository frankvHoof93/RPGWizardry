using System;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        #region InnerTypes
        /// <summary>
        /// Table that holds Loot-Values for the different droppable Items
        /// </summary>
        [Serializable]
        public struct LootTable
        {
            [SerializeField]
            public LootSpawn dust;
            [SerializeField]
            public LootSpawn gold;
            [SerializeField]
            public LootSpawn page;
            [SerializeField]
            public LootSpawn potion;
        }
        /// <summary>
        /// Chance-Value for spawning Loot
        /// </summary>
        [Serializable]
        public struct LootSpawn
        {
            [SerializeField]
            public uint amount;
            [SerializeField]
            [Range(0, 1)]
            public float chance;
        }
        #endregion

        #region Public
        public LootTable Loot => droppedLoot;
        public string Name => enemyName;
        public ushort Health => enemyHealth;
        public ushort Attack => enemyAttack;
        public float Speed => enemySpeed;
        #endregion

        #region Editor
        [SerializeField]
        private string enemyName;
        [SerializeField]
        private ushort enemyHealth;
        [SerializeField]
        private ushort enemyAttack;
        [SerializeField]
        private float enemySpeed;
        [SerializeField]
        private LootTable droppedLoot;
        #endregion
    }
}
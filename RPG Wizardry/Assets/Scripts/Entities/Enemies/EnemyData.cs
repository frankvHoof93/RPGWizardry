using nl.SWEG.RPGWizardry.Utils.DataTypes;
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
        /// <summary>
        /// Name of this Enemy
        /// </summary>
        public string Name => enemyName;
        /// <summary>
        /// Base Health for this Enemy
        /// </summary>
        public ushort Health => enemyHealth;
        /// <summary>
        /// Base Attack for this Enemy
        /// </summary>
        public ushort Attack => enemyAttack;
        /// <summary>
        /// Base Speed for this Enemy
        /// </summary>
        public float Speed => enemySpeed;
        /// <summary>
        /// Cooldown after Spawning for this Enemy (time until its AI and attacks are enabled)
        /// </summary>
        public FloatRange SpawnCooldown => spawnCooldown;
        /// <summary>
        /// Loot that can be dropped by this Enemy
        /// </summary>
        public LootTable Loot => droppedLoot;
        #endregion

        #region Editor
        /// <summary>
        /// Name of this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Name of this Enemy")]
        private string enemyName;
        /// <summary>
        /// Base Health for this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Base Health for this Enemy")]
        private ushort enemyHealth;
        /// <summary>
        /// Base Attack for this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Base Attack for this Enemy")]
        private ushort enemyAttack;
        /// <summary>
        /// Base Speed for this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Base Speed for this Enemy")]
        private float enemySpeed;
        /// <summary>
        /// Cooldown after Spawning for this Enemy (time until its AI and attacks are enabled)
        /// </summary>
        [SerializeField]
        [Tooltip("Cooldown after Spawning for this Enemy (time until its AI and attacks are enabled)")]
        protected FloatRange spawnCooldown;
        /// <summary>
        /// Loot that can be dropped by this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Loot that can be dropped by this Enemy")]
        private LootTable droppedLoot;
        #endregion
    }
}
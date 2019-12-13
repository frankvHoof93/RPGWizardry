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
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        public int OpacityPriority => opacityPriority;
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        public float OpacityRadius => opacityRadius;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        public Vector2 OpacityOffset => opacityOffset;
        #endregion

        #region Editor
        [Header("Stats")]
        #region Stats
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
        #endregion

        [Header("Spawning")]
        #region Spawning
        /// <summary>
        /// Cooldown after Spawning for this Enemy (time until its AI and attacks are enabled)
        /// </summary>
        [SerializeField]
        [Tooltip("Cooldown after Spawning for this Enemy (time until its AI and attacks are enabled)")]
        protected FloatRange spawnCooldown;
        #endregion

        [Header("Loot")]
        #region Loot
        /// <summary>
        /// Loot that can be dropped by this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Loot that can be dropped by this Enemy")]
        private LootTable droppedLoot;
        #endregion

        [Header("Opacity")]
        #region Opacity
        /// <summary>
        /// Priority for rendering Opacity
        /// </summary>
        [SerializeField]
        [Range(1, 10000)]
        [Tooltip("Priority for rendering Opacity")]
        private int opacityPriority = 1;
        /// <summary>
        /// Opacity-Radius in Pixels (for 720p)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Radius in Pixels (for 720p)")]
        private float opacityRadius;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Offset from Transform (in World-Space)")]
        private Vector2 opacityOffset;
        #endregion
        #endregion
    }
}
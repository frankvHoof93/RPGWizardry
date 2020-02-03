using nl.SWEG.Willow.Utils.DataTypes;
using System;
using UnityEngine;

namespace nl.SWEG.Willow.Entities.Enemies
{
    /// <summary>
    /// Scriptable Object containing default (base) Data for Enemy
    /// </summary>
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
            /// <summary>
            /// Loot-Values for Dust-Pile
            /// </summary>
            [SerializeField]
            [Tooltip("Loot-Values for Dust-Pile")]
            public LootSpawn dust;
            /// <summary>
            /// Loot-Values for Gold-Pile
            /// </summary>
            [SerializeField]
            [Tooltip("Loot-Values for Gold-Pile")]
            public LootSpawn gold;
            //[SerializeField] // Removed, as Pages require Spells to spawn (not every enemy has a spell)
            //public LootSpawn page;
            /// <summary>
            /// Loot-Values for Health-Potion(s)
            /// </summary>
            [SerializeField]
            [Tooltip("Loot-Values for Health-Potion(s)")]
            public LootSpawn potion;           
        }
        /// <summary>
        /// Chance-Value for spawning Loot
        /// </summary>
        [Serializable]
        public struct LootSpawn
        {
            /// <summary>
            /// Amount for spawned Loot
            /// </summary>
            [SerializeField]
            [Tooltip("Amount for spawned Loot")]
            public uint amount;
            /// <summary>
            /// Chance for spawning Loot (0-1)
            /// </summary>
            [SerializeField]
            [Range(0, 1)]
            [Tooltip("Chance for spawning Loot (0-1)")]
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
        /// Base Knockback for this Enemy
        /// </summary>
        public int Knockback => knockback;
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
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
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
        /// <summary>
        /// Base Knockback for this Enemy
        /// </summary>
        [SerializeField]
        [Tooltip("Base Speed for this Enemy")]
        private int knockback;
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
        /// Opacity-Radius in Pixels (for 720p-Resolution)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Radius in Pixels (for 720p-Resolution)")]
        private float opacityRadius;
        /// <summary>
        /// Opacity-Offset from Transform (in World-Space)
        /// </summary>
        [SerializeField]
        [Tooltip("Opacity-Offset from Transform (in World-Space)")]
        private Vector2 opacityOffset;
        #endregion
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.Willow.Sorcery.Spells
{
    /// <summary>
    /// Holds (Default) Data for a Spell
    /// </summary>
    [CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData", order = 1)]
    public class SpellData : ScriptableObject
    {
        #region Variables
        /// <summary>
        /// Name for Spell
        /// </summary>
        public string Name => spellName;
        [SerializeField]
        [Tooltip("Name for Spell")]
        private string spellName;
        /// <summary>
        /// Description for Spell
        /// </summary>
        public string Description => spellDescription;
        [SerializeField]
        [Tooltip("Description for Spell")]
        private string spellDescription;
        /// <summary>
        /// Cost in Dust for Spell-Unlocking
        /// </summary>
        public ushort SpellCost => spellCost;
        [SerializeField]
        [Tooltip("Cost in Dust for Spell-Unlocking")]
        private ushort spellCost;
        /// <summary>
        /// Pattern for Projectiles
        /// </summary>
        public SpellPattern Pattern => spellPattern;
        [SerializeField]
        [Tooltip("Pattern for Projectiles")]
        private SpellPattern spellPattern;
        /// <summary>
        /// Amount of Damage inflicted by Spell (per Projectile)
        /// </summary>
        public ushort Damage => spellDamage;
        [SerializeField]
        [Tooltip("Amount of Damage inflicted by Spell (per Projectile)")]
        private ushort spellDamage;
        /// <summary>
        /// Element-Type for Spell (for Elemental Effects)
        /// </summary>
        public Element Element => spellElement;
        [SerializeField]
        [Tooltip("Element-Type for Spell (for Elemental Effects)")]
        private Element spellElement;
        /// <summary>
        /// Cooldown between Casts
        /// </summary>
        public float Cooldown => spellCooldown;
        [SerializeField]
        [Tooltip("Cooldown between Casts")]
        private float spellCooldown;
        /// <summary>
        /// LifeTime for Projectiles (Time after which Projectiles are destroyed)
        /// </summary>
        public float LifeTime => projectileLifeTime;
        [SerializeField]
        [Tooltip("LifeTime for Projectiles (Time after which Projectiles are destroyed)")]
        private float projectileLifeTime;
        /// <summary>
        /// Speed for Projectiles
        /// </summary>
        public float ProjectileSpeed => projectileSpeed;
        [SerializeField]
        [Tooltip("Speed for Projectiles")]
        private float projectileSpeed;
        /// <summary>
        /// Knockback for Projectiles
        /// </summary>
        public int Knockback => knockback;
        [SerializeField]
        [Tooltip("Knockback for Projectiles")]
        private int knockback;
        /// <summary>
        /// UI-Sprite for Spell
        /// </summary>
        public Sprite Sprite => spellSprite;
        [SerializeField]
        [Tooltip("UI-Sprite for Spell")]
        private Sprite spellSprite;
        /// <summary>
        /// UI-Sprite for Spell-Cooldown
        /// </summary>
        public Sprite CooldownSprite => cooldownSprite;
        [SerializeField]
        [Tooltip("UI-Sprite for Spell-Cooldown")]
        private Sprite cooldownSprite;
        /// <summary>
        /// AudioClip for spell spawn
        /// </summary>
        public AudioClip SpawnClip => spawnClip;
        [SerializeField]
        [Tooltip("AudioClip for Spell-Spawn")]
        private AudioClip spawnClip;
        /// <summary>
        /// AudioClip for spell impact
        /// </summary>
        public AudioClip ImpactClip => impactClip;
        [SerializeField]
        [Tooltip("AudioClip for Spell-Impact")]
        private AudioClip impactClip;
        /// <summary>
        /// Prefab for Projectile
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab for Projectile")]
        private GameObject projectilePrefab;
        #endregion

        #region Methods
        /// <summary>
        /// Spawns Projectiles for Spell
        /// TODO: Add more SpellPatterns
        /// </summary>
        /// <param name="position">(Base) position for spawning</param>
        /// <param name="direction">Direction in which spell is Cast</param>
        /// <param name="targetingMask">LayerMask for Projectile-Collisions</param>
        /// <returns>List of Spawned Projectiles</returns>
        public List<Projectile> SpawnSpell(Vector2 position, Vector2 direction, LayerMask targetingMask)
        {
            List<Projectile> returnVal = new List<Projectile>();
            direction.Normalize();
            switch (spellPattern)
            {
                case SpellPattern.Line:
                    GameObject projectile = Instantiate(projectilePrefab);
                    projectile.transform.position = position;
                    projectile.transform.up = direction;
                    Projectile p = projectile.GetComponent<Projectile>();
                    p.SetData(this, targetingMask);
                    returnVal.Add(p);
                    break;
                case SpellPattern.Cone:
                    break;
                case SpellPattern.Circle:
                    break;
                default:
                    break;
            }
            return returnVal;
        }
        #endregion
    }
}
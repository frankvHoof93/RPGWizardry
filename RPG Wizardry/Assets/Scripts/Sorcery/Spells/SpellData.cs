using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData", order = 1)]
    public class SpellData : ScriptableObject
    {
        public string Name => spellName;
        public SpellPattern Pattern => spellPattern;
        public ushort Damage => spellDamage;
        public Element Element => spellElement;
        public float Cooldown => spellCooldown;
        public float LifeTime => projectileLifeTime;
        public float ProjectileSpeed => projectileSpeed;

        [SerializeField]
        private string spellName;
        [SerializeField]
        private SpellPattern spellPattern;
        /// <summary>
        /// Damage per projectile
        /// </summary>
        [SerializeField]
        private ushort spellDamage;
        [SerializeField]
        private float projectileSpeed;
        [SerializeField]
        private float projectileLifeTime;
        [SerializeField]
        private Element spellElement;
        [SerializeField]
        private float spellCooldown;
        [SerializeField]
        private GameObject projectilePrefab;

        public List<Projectile> SpawnSpell(Vector2 position, Vector2 direction, LayerMask targetingMask)
        {
            List<Projectile> returnVal = new List<Projectile>();
            direction.Normalize();
            switch (spellPattern)
            {
                case SpellPattern.line:
                    GameObject projectile = Instantiate(projectilePrefab);
                    projectile.transform.position = position;
                    projectile.transform.up = direction;
                    Projectile p = projectile.GetComponent<Projectile>();
                    p.SetData(this, targetingMask);
                    Destroy(projectile, projectileLifeTime);
                    returnVal.Add(p);
                    break;
                case SpellPattern.cone:
                    break;
                case SpellPattern.circle:
                    break;
                default:
                    break;
            }
            return returnVal;
        }
    }
}
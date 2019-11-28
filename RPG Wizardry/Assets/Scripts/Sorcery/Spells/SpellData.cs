using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    [CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData", order = 1)]
    public class SpellData : ScriptableObject
    {
        public string Name => spellName;
        public SpellPattern Pattern => spellPattern;
        public int Range => spellRange;
        public int Damage => spellDamage;
        public Element Element => spellElement;
        public int Cooldown => spellCooldown;

        [SerializeField]
        private string spellName;
        [SerializeField]
        private SpellPattern spellPattern;
        [SerializeField]
        private int spellRange;
        [SerializeField]
        private int spellDamage;
        [SerializeField]
        private Element spellElement;
        [SerializeField]
        private int spellCooldown;
    }
}
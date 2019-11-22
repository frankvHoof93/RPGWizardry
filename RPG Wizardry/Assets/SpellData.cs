using System.Collections;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.Sorcery;
using UnityEngine;

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
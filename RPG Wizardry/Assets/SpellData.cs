using System.Collections;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.Sorcery;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    public string spellName;
    public SpellPattern spellPattern;
    public int spellRange;
    public int spellDamage;
    public Element spellElement;
    public int spellCooldown;
}
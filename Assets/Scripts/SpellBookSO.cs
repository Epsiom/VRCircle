using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellBook", menuName = "ScriptableObjects/New SpellBookSO")]
public class SpellBookSO : ScriptableObject
{
    [SerializeField] public List<SpellBookEntry> spellBookEntries = new List<SpellBookEntry>();

    // Returns the spell matching the input
    public SpellBookEntry getSpellObject(string input)
    {
        foreach (SpellBookEntry spellBookEntry in spellBookEntries)
        {
            if (spellBookEntry.input == input)
            {
                return spellBookEntry;
            }
        }
        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellParser : MonoBehaviour
{
    public class SpellInformations  //TODO: put in a separate file and refine the concept
    {
        public SpellElement SpellElement;
        public SpellShape SpellShape;

        public SpellInformations(SpellElement newSpellElement, SpellShape newSpellShape)
        {
            this.SpellElement = newSpellElement;
            this.SpellShape = newSpellShape;
        }
    }

    // Starting from the bottom of the numpad, it's the list of all numbers in a counter-clockwise rotation
    private List<char> ORDERED_NUMPAD_NOTATIONS = new List<char>() { '2', '3', '6', '9', '8', '7', '4', '1' };

    private List<string> QCF_LIST = new List<string>() { "236", "369", "698", "987", "874", "741", "412", "123" };
    private List<string> QCB_LIST = new List<string>() { "214", "147", "478", "789", "896", "963", "632", "321" };
    private List<string> ZF_LIST = new List<string>() { "623", "936", "869", "798", "487", "174", "241", "312" };
    private List<string> ZB_LIST = new List<string>() { "421", "714", "847", "978", "689", "396", "263", "132" };

    public SpellInformations ParseSpell(string spellString)
    {
        if (spellString.Length < 3) return null;

        // First, the initial character is for the element
        Enum.TryParse<SpellElement>(spellString.Substring(0,1), out SpellElement spellElement);

        // Then, the three first characters (first included) are parsed for the shape of the spell
        string spellShapeStr = spellString.Substring(0, 2);
        SpellShape spellShape = SpellShape.UNDEFINED;
        if (QCF_LIST.Contains(spellShapeStr)) spellShape = SpellShape.PROJECTILE;
        if (QCB_LIST.Contains(spellShapeStr)) spellShape = SpellShape.SHARDS;
        if (ZF_LIST.Contains(spellShapeStr)) spellShape = SpellShape.BEAM;
        if (ZB_LIST.Contains(spellShapeStr)) spellShape = SpellShape.WALL;

        if (spellShape == SpellShape.UNDEFINED) return null;

        return new SpellInformations(spellElement, spellShape);
    }

}

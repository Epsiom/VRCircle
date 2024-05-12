using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpellParser
{
    // Starting from the bottom of the numpad, it's the list of all numbers in a counter-clockwise rotation
    private static readonly List<char> ORDERED_NUMPAD_NOTATIONS = new List<char>() { '2', '3', '6', '9', '8', '7', '4', '1' };

    private static readonly List<string> QCF_LIST = new List<string>() { "236", "369", "698", "987", "874", "741", "412", "123" };
    private static readonly List<string> QCB_LIST = new List<string>() { "214", "147", "478", "789", "896", "963", "632", "321" };
    private static readonly List<string> ZF_LIST = new List<string>() { "623", "936", "869", "798", "487", "174", "241", "312" };
    private static readonly List<string> ZB_LIST = new List<string>() { "421", "714", "847", "978", "689", "396", "263", "132" };

    public static GameObject ParseAndBuildSpell(string spellString)
    {
        if (spellString.Length < 3) return null;

        // First, the initial character is for the element
        Enum.TryParse<SpellElement>(spellString.Substring(0,1), out SpellElement spellElement);

        // Then, the three first characters (first included) are parsed for the shape of the spell
        string spellShapeStr = spellString.Substring(0, 3);
        Debug.Log(spellShapeStr);
        SpellShape spellShape = SpellShape.UNDEFINED;
        if (QCF_LIST.Contains(spellShapeStr)) spellShape = SpellShape.PROJECTILE;
        if (QCB_LIST.Contains(spellShapeStr)) spellShape = SpellShape.SHARDS;
        if (ZF_LIST.Contains(spellShapeStr)) spellShape = SpellShape.BEAM;
        if (ZB_LIST.Contains(spellShapeStr)) spellShape = SpellShape.WALL;

        Debug.Log(spellElement);
        Debug.Log(spellShape);

        if (spellShape == SpellShape.UNDEFINED) return null;

        return buildSpell(spellElement, spellShape);
    }

    private static GameObject buildSpell(SpellElement spellElement, SpellShape spellShape)
    {
        GameObject spellShapeFlyweightObject = GlobalReferences.Instance.SpellFlyweightsSO.getShapeFlyweight(spellShape).spellObjectPrefab;
        GameObject spellElementFlyweightObject = GlobalReferences.Instance.SpellFlyweightsSO.getElementFlyweight(spellElement).spellObjectPrefab;

        GameObject spell = GameObject.Instantiate(spellShapeFlyweightObject, GlobalReferences.Instance.SpellSpawningPoint.position, GlobalReferences.Instance.SpellSpawningPoint.rotation, null);
        switch (spellShape)
        {
            case SpellShape.PROJECTILE:
                ProjectileSpellBehaviour projectileSpellBehaviour = spell.GetComponent<ProjectileSpellBehaviour>();
                projectileSpellBehaviour.InitElement(spellElementFlyweightObject);
                break;
            case SpellShape.BEAM:
                BeamSpellBehaviour beamSpellBehaviour = spell.GetComponent<BeamSpellBehaviour>();
                beamSpellBehaviour.InitElement(spellElementFlyweightObject);
                break;
            case SpellShape.WALL:
                //TODO
                break;
            case SpellShape.SHARDS:
                //TODO
                break;
            default:
                return null;
        }
        return spell;
    }
}

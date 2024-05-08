using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellFlyweightsSO", menuName = "ScriptableObjects/New SpellFlyweightsSO")]
public class SpellFlyweightsSO : ScriptableObject
{
    [SerializeField] public List<SpellShapeFlyweight> _spellShapeFlyweights = new List<SpellShapeFlyweight>();
    [SerializeField] public List<SpellElementFlyweight> _spellElementFlyweights = new List<SpellElementFlyweight>();

    // Returns the spell shape flyweight of the matching shape
    public SpellShapeFlyweight getShapeFlyweight(SpellShape shape)
    {
        foreach (SpellShapeFlyweight spellShapeFlyweight in _spellShapeFlyweights)
        {
            if (spellShapeFlyweight.shape == shape)
            {
                return spellShapeFlyweight;
            }
        }
        return null;
    }

    // Returns the spell element flyweight of the matching element
    public SpellElementFlyweight getElementFlyweight(SpellElement element)
    {
        foreach (SpellElementFlyweight spellElementFlyweight in _spellElementFlyweights)
        {
            if (spellElementFlyweight.element == element)
            {
                return spellElementFlyweight;
            }
        }
        return null;
    }
}

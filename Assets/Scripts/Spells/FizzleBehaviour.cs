using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzleBehaviour : SpellBehaviour
{
    private void Start()
    {
        CastCountdown = 0;
    }

    protected override void CastSpell()
    {
        //TODO: Does nothing
        Debug.Log("FIZZLE");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    protected float CastCountdown = 1f; // Time in seconds after which the spell is cast
    protected bool _IsSpellCast = false;

    protected abstract void CastSpell();

    protected virtual void Update()
    {
        if (CastCountdown >= 0)
        {
            CastCountdown -= Time.deltaTime;
        }
        else
        {
            return;
        }

        if (CastCountdown <= 0)
        {
            _IsSpellCast = true;
            CastSpell();
        }
    }
}

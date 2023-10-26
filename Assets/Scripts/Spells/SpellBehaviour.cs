using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    protected double CastCountdown = 1; // Time in seconds after which the spell is cast

    protected abstract void CastSpell();

    void Update()
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
            CastSpell();
        }
    }
}

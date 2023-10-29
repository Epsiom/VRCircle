using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzleBehaviour : SpellBehaviour
{
    private Unity.VRTemplate.LaunchProjectile _LaunchProjectile;

    private void Start()
    {
        CastCountdown = 0;
        _LaunchProjectile = this.GetComponent<Unity.VRTemplate.LaunchProjectile>();
    }

    protected override void CastSpell()
    {
        _LaunchProjectile.Fire();
        Destroy(this.gameObject);
    }
}

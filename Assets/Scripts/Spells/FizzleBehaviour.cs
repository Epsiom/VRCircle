using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzleBehaviour : BaseSpellBehaviour
{
    private Unity.VRTemplate.LaunchProjectile _LaunchProjectile;

    protected override void Start()
    {
        CastCountdownTimer = 0;
        _LaunchProjectile = this.GetComponent<Unity.VRTemplate.LaunchProjectile>();

        base.Start();
    }

    protected override void CastSpell()
    {
        _LaunchProjectile.Fire();
        Destroy(this.gameObject);
    }
}

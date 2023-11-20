using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellShape
{
    UNDEFINED,
    PROJECTILE,     // One single projectile
    SHARDS,         // A shotgun spread of smaller projectiles
    BEAM,           // A continuous beam
    WALL            // A wall to either damage enemies or block projectiles
}

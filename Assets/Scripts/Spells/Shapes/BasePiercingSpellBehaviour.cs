using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base of all 'piercing' spells, which means the spells that pass through enemies, dealing continuous damage over time
/// The opposite of a piercing spell is a solid spell, which impacts an enemy to deal damage once and disappear on contact
/// </summary>
public abstract class BasePiercingSpellBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The layer of the enemy characters")]
    protected LayerMask TargetLayer;

    [SerializeField]
    [Tooltip("The damage delt by the spell")]
    protected int _SpellDamage = 1;

    [SerializeField] protected float CastCountdownTimer = 1f; // Time in seconds after which the spell is cast

    protected CapsuleCollider _Collider;
    //protected bool _IsSpellCast = false;

    protected abstract void CastSpell();

    protected virtual void Start()
    {
        _Collider = this.GetComponent<CapsuleCollider>();

        Invoke(nameof(CastSpell), CastCountdownTimer);
    }

    private void Update()
    {
        // Perform overlap check
        Collider[] colliders = Physics.OverlapCapsule(
            _Collider.bounds.center - new Vector3(0, _Collider.height / 2 - _Collider.radius, 0), // Start position of the capsule
            _Collider.bounds.center + new Vector3(0, _Collider.height / 2 - _Collider.radius, 0), // End position of the capsule
            _Collider.radius, // Radius of the capsule
            TargetLayer
        );
        /*
        Collider[] colliders = Physics.OverlapBox(
            _Collider.bounds.center, // Center of your collider
            _Collider.bounds.extents, // Half extents of your collider
            _Collider.transform.rotation,
            TargetLayer
        );*/
        foreach (Collider enemyCollider in colliders)
        {
            BaseEnemyBehaviour enemy = enemyCollider.gameObject.GetComponent<BaseEnemyBehaviour>();
            enemy.DamageHealth(_SpellDamage);
        }
    }
}

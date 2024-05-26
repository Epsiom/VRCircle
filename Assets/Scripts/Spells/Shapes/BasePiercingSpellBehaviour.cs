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

    protected Collider _Collider;
    //protected bool _IsSpellCast = false;

    protected abstract void CastSpell();

    protected virtual void Start()
    {
        _Collider = this.GetComponent<Collider>();

        Invoke(nameof(CastSpell), CastCountdownTimer);
    }

    private void Update()
    {
        // Performs the overlap check
        Collider[] colliders;
        if (_Collider is CapsuleCollider)
        {
            CapsuleCollider capsuleCollider = (CapsuleCollider)_Collider;
            colliders = Physics.OverlapCapsule(
                capsuleCollider.bounds.center - new Vector3(0, capsuleCollider.height / 2 - capsuleCollider.radius, 0), // Start position of the capsule
                capsuleCollider.bounds.center + new Vector3(0, capsuleCollider.height / 2 - capsuleCollider.radius, 0), // End position of the capsule
                capsuleCollider.radius, // Radius of the capsule
                TargetLayer
            );
        }
        else //if (_Collider is SphereCollider)
        {
            SphereCollider sphereCollider = (SphereCollider)_Collider;
            Vector3 center = transform.position + sphereCollider.center;    // Calculates the center of the SphereCollider in world space
            float radius = sphereCollider.radius;
            colliders = Physics.OverlapSphere(center, radius, TargetLayer);
        }
        foreach (Collider enemyCollider in colliders)
        {
            BaseEnemyBehaviour enemy = enemyCollider.gameObject.GetComponent<BaseEnemyBehaviour>();
            enemy.DamageHealth(_SpellDamage);
        }
    }
}

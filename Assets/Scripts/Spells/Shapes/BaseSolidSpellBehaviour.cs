using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base of all 'solid' spells, which means the spells that impact something and inflict damage on contact then disappear
/// The opposite of a solid spell is a piercing spell, which means one that deals continuous damage over time
/// </summary>
public abstract class BaseSolidSpellBehaviour : MonoBehaviour
{
    [SerializeField] public float CastCountdownTimer = 1f;               // Time in seconds after which the spell is cast
    [SerializeField] public float SpellDestructionTimer = 5f;            // Time in seconds after which the casted spell is destroyed
    [SerializeField] public float SpellDestructionProcessTimer = 0.25f;  // Time in seconds taken to shrink then destroy the spell

    [SerializeField]
    [Tooltip("The layer of the enemy characters")]
    protected LayerMask TargetLayer;

    [SerializeField]
    [Tooltip("The damage delt by the spell")]
    protected int _SpellDamage = 1;

    protected abstract void CastSpell();

    protected virtual void Start()
    {
        Invoke(nameof(CastSpell), CastCountdownTimer);
        Invoke(nameof(StartSpellDestruction), CastCountdownTimer + SpellDestructionTimer);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.collider.gameObject.layer) & TargetLayer) != 0)
        {
            BaseEnemyBehaviour baseCharacterBehaviour = collision.gameObject.GetComponent<BaseEnemyBehaviour>();
            if (baseCharacterBehaviour == null)
            {
                Debug.LogError("The collided character (" + collision.gameObject.name + ") does not have a baseCharacterBehaviour.");
                return;
            }
            baseCharacterBehaviour.DamageHealth(_SpellDamage);
            DestroySpell();
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (TryGetComponent(out IDamageable damageable))
            damageable.Damage(_SpellDamage);
    
        Destroy(this.gameObject);
    }
    */

    private void StartSpellDestruction()
    {
        this.transform.DOScale(Vector3.zero, SpellDestructionProcessTimer);
        Invoke(nameof(DestroySpell), SpellDestructionProcessTimer);
    }
    private void DestroySpell()
    {
        DOTween.Kill(gameObject);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The layer of the character")]
    private LayerMask _TargetLayer;

    [SerializeField]
    [Tooltip("The damage delt by the spell")]
    private int _SpellDamage = 1;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.collider.gameObject.layer) & _TargetLayer) != 0)
        {
            BaseCharacterBehaviour baseCharacterBehaviour = collision.gameObject.GetComponent<BaseCharacterBehaviour>();
            if (baseCharacterBehaviour == null)
            {
                Debug.LogError("The collided character (" + collision.gameObject.name + ") does not have a baseCharacterBehaviour.");
                return;
            }
            baseCharacterBehaviour.DamageHealth(_SpellDamage);
            Destroy(this.gameObject);
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
}

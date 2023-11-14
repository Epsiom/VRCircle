using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpellBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The layer of the character")]
    protected LayerMask TargetLayer;

    [SerializeField]
    [Tooltip("The damage delt by the spell")]
    private int _SpellDamage = 1;

    [SerializeField] protected float CastCountdown = 1f; // Time in seconds after which the spell is cast
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

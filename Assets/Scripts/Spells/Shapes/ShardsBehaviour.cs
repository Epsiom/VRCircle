using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardsBehaviour : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The damage delt by the spell")]
    protected int _SpellQuantity = 8;

    public void Init(SpellElementFlyweight spellElementFlyweight, GameObject projectileSpellShapeFlyweightObject)
    {
        for (int i=0; i<_SpellQuantity; i++)
        {
            GameObject spell = GameObject.Instantiate(
                projectileSpellShapeFlyweightObject,
                GlobalReferences.Instance.SpellSpawningPoint.position,
                GlobalReferences.Instance.SpellSpawningPoint.rotation,
                null);
            ProjectileSpellBehaviour projectileSpellBehaviour = spell.GetComponent<ProjectileSpellBehaviour>();
            projectileSpellBehaviour.InitElement(spellElementFlyweight);

            projectileSpellBehaviour.ProjectileDeviationAngle = 7.5f;
            projectileSpellBehaviour.SpellDestructionTimer = 0.25f;
            projectileSpellBehaviour.SpellDestructionProcessTimer = 0.05f;
            projectileSpellBehaviour.ProjectileSpeed *= 2;
            projectileSpellBehaviour.InitialScale /= 2;
            projectileSpellBehaviour.FinalScale /= 2;
        }
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class BeamSpellBehaviour : BasePiercingSpellBehaviour
{
    [SerializeField] protected float BeamStartupDuration = 1f; // Time in seconds after which the collider becomes active
    [SerializeField] protected float BeamActiveDuration = 1f;  // Time in seconds after which the beam ends

    private VisualEffect _visualEffect;

    // Start is called before the first frame update
    protected override void Start()
    {
        CastCountdownTimer = 0f;                                               // The beam growing in size is handled by the vfx
        base.Start();
        _Collider.enabled = false;
        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand
    }

    /// <summary>
    /// Adds the components of the element flyweight prefab to the currently blank projectile (texture, trails.. etc)
    /// </summary>
    public void InitElement(SpellElementFlyweight spellElementFlyweight)
    {
        _visualEffect = GetComponent<VisualEffect>();
        _visualEffect.SetVector4("CoreColor", spellElementFlyweight.PrimaryColor);
    }

    /// <summary>
    /// Starts the beam VFX
    /// </summary>
    protected override void CastSpell()
    {
        transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        _visualEffect.Play();
        Invoke(nameof(StartBeamActiveState), BeamStartupDuration);
    }

    /// <summary>
    /// Starts the beam collision, to sync up with the vfx visuals
    /// </summary>
    private void StartBeamActiveState()
    {
        _Collider.enabled = true;
        Invoke(nameof(EndBeamActiveState), BeamActiveDuration);
    }

    private void EndBeamActiveState()
    {
        _Collider.enabled = false;
        Destroy(this.gameObject);
    }
}

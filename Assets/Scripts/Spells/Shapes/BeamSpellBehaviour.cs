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
        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand
    }

    /// <summary>
    /// Adds the components of the element flyweight prefab to the currently blank projectile (texture, trails.. etc)
    /// </summary>
    public void InitElement(SpellElementFlyweight spellElementFlyweight)
    {
        _visualEffect = GetComponent<VisualEffect>();
        _visualEffect.SetVector4("CoreColor", spellElementFlyweight.PrimaryColor);
        _visualEffect.SetMesh("OriginSpinnerMesh", spellElementFlyweight.PrimaryMesh);
    }

    /// <summary>
    /// Starts the beam VFX (the beam is slim, and does not inflict damage yet as long as it is in its startup duration)
    /// </summary>
    protected override void CastSpell()
    {
        //TODO: move to the Start(), and use CastCountdownTimer instead of BeamStartupDuration

        transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        _visualEffect.Play();
        Invoke(nameof(StartBeamActiveState), BeamStartupDuration);
    }

    /// <summary>
    /// Starts the beam collision, to sync up with the vfx visuals
    /// </summary>
    private void StartBeamActiveState()
    {
        _IsSpellActive = true;
        Invoke(nameof(EndBeamActiveState), BeamActiveDuration);
    }

    private void EndBeamActiveState()
    {
        _IsSpellActive = false;
        Destroy(this.gameObject);
    }
}

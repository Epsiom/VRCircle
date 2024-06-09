using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartSpellBehaviour : BaseSolidSpellBehaviour
{
    public float ProjectileSpeed = 2f;
    public float ReverseGravityFactor = 0.1f;

    public float InitialScale = 0.0015f;    // Initial scale of the spell's GameObject when first instanciated, after which it grows to FinalScale
    public float FinalScale = 1f;           // Final scale of the spell's GameObject, reached just before its cast and leaves the wand

    private Rigidbody _Rigidbody;

    // Start is called before the first frame update
    protected override void Start()
    {
        _Rigidbody = this.GetComponent<Rigidbody>();

        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        this.transform.DOScale(new Vector3(FinalScale, FinalScale, FinalScale), this.CastCountdownTimer);

        CastCountdownTimer = 0.5f;
        SpellDestructionTimer = 1f;
        base.Start();
    }

    private void FixedUpdate()
    {
        _Rigidbody.AddForce(-ReverseGravityFactor * Physics.gravity, ForceMode.Acceleration);
    }

    protected override void CastSpell()
    {
        this.transform.SetParent(null);
        Vector3 directionVector = (this.transform.position - GlobalReferences.Instance.Wand.position);   // The direction from the wand to the spell's position
        directionVector.Normalize();
        _Rigidbody.velocity = directionVector * ProjectileSpeed;

        // Starts the wave when the spell starts disappearing
        Invoke(nameof(StartWave), SpellDestructionTimer);
    }

    private void StartWave()
    {
        WaveHandler.Instance.StartNewWave();
    }
}

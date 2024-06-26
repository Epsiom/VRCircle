using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AuraSpellBehaviour : BasePiercingSpellBehaviour
{
    public float ProjectileSpeed = 50f;
    public float ProjectileDeviationAngle = 0f;
    public float InitialScale = 0.0015f;    // Initial scale of the spell's GameObject when first instanciated, after which it grows to FinalScale
    public float FinalScale = 0.15f;        // Final scale of the spell's GameObject, reached just before its cast and leaves the wand

    [SerializeField] public float SpellActivationTimer = 0.25f;          // Time in seconds after which the aura spell raises in size
    [SerializeField] public float SpellDestructionTimer = 2f;            // Time in seconds after which the casted spell is destroyed
    [SerializeField] public float SpellDestructionProcessTimer = 0.25f;  // Time in seconds taken to shrink then destroy the spell

    private Rigidbody _Rigidbody;

    private bool _isWithGravity = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        _Rigidbody = this.GetComponent<Rigidbody>();
        //_Rigidbody.excludeLayers = 1 << GlobalReferences.Instance.Player      // TODO: and the wand..?

        _IsSpellActive = false;

        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand until it fires

        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        this.transform.DOScale(new Vector3(FinalScale, FinalScale, FinalScale), this.CastCountdownTimer);
        
        base.Start();
    }
    
    /// <summary>
    /// Changes the values of the components of the spell to match the ones indicated in the element flyweight (texture, trails.. etc)
    /// </summary>
    public void InitElement(SpellElementFlyweight spellElementFlyweight)
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        ParticleSystemRenderer particleSystemRenderer = this.GetComponent<ParticleSystemRenderer>();

        meshFilter.mesh = spellElementFlyweight.PrimaryMesh;
        meshRenderer.material = spellElementFlyweight.PrimaryMaterial;
        particleSystemRenderer.material = spellElementFlyweight.PrimaryMaterial;
        _isWithGravity = spellElementFlyweight.IsWithGravity;
        FinalScale = spellElementFlyweight.PrimaryMeshScale;
        ProjectileSpeed = spellElementFlyweight.ProjectileSpeed;
    }

    protected override void CastSpell()
    {
        this.transform.SetParent(null);
        _IsSpellActive = true;

        Vector3 directionVector = (this.transform.position - GlobalReferences.Instance.Wand.position);   // The direction from the wand to the spell's position
        directionVector.Normalize();

        if (ProjectileDeviationAngle > 0)
        {
            Quaternion deviationRotation = Quaternion.Euler(Random.Range(-ProjectileDeviationAngle, ProjectileDeviationAngle), Random.Range(-ProjectileDeviationAngle, ProjectileDeviationAngle), 0);
            _Rigidbody.velocity = deviationRotation * directionVector * ProjectileSpeed;
        }
        else
        {
            _Rigidbody.velocity = directionVector * ProjectileSpeed;
        }
        _Rigidbody.useGravity = _isWithGravity;

        Invoke(nameof(StartAuraActiveState), SpellActivationTimer);
    }

    /// <summary>
    /// Starts the aura collision, raises the projectile size, and slows it down massively
    /// </summary>
    private void StartAuraActiveState()
    {
        _Rigidbody.velocity /= 20;
        this.transform.DOScale(FinalScale*20, SpellDestructionProcessTimer);
        Invoke(nameof(EndAuractiveState), SpellDestructionTimer);
    }
    private void EndAuractiveState()
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

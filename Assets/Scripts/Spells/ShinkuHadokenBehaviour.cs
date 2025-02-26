using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//TODO: Replace in the prefabs by ProjectileSpellBehaviour, then remove

public class ShinkuHadokenBehaviour : BaseSolidSpellBehaviour
{
    public float ProjectileSpeed = 25f;
    public float InitialScale = 0.0015f;
    public float FinalScale = 0.5f;

    private Rigidbody _Rigidbody;

    // Start is called before the first frame update
    protected override void Start()
    {
        this.CastCountdownTimer = 2f;

        _Rigidbody = this.GetComponent<Rigidbody>();
        this.transform.SetParent(GlobalReferences.Instance.Wand);    //Attaches the spell to the wand until it fires

        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        this.transform.DOScale(new Vector3(FinalScale, FinalScale, FinalScale), this.CastCountdownTimer);

        base.Start();
    }

    /*
    protected override void Update()
    {
        base.Update();
    }*/

    protected override void CastSpell()
    {
        this.transform.SetParent(null);

        Vector3 directionVector = (this.transform.position - GlobalReferences.Instance.Wand.position);   // The direction from the wand to the spell's position
        directionVector.Normalize();
        _Rigidbody.velocity = directionVector * ProjectileSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HadokenBehaviour : SpellBehaviour
{
    public float ProjectileSpeed = 50f;
    public float InitialScale = 0.0015f;
    public float FinalScale = 0.15f;

    private Rigidbody _Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        this.CastCountdown = 0.5f;

        _Rigidbody = this.GetComponent<Rigidbody>();
        //_Rigidbody = this.GetComponent<Rigidbody>();
        //_Rigidbody.excludeLayers = 1 << GlobalReferences.Instance.Player      // TODO: and the wand..?

        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand until it fires

        this.transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        this.transform.DOScale(new Vector3(FinalScale, FinalScale, FinalScale), this.CastCountdown);
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

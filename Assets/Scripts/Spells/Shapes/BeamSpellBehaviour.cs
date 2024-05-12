using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class BeamSpellBehaviour : BasePiercingSpellBehaviour
{
    private VisualEffect _visualEffect;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _visualEffect = GetComponent<VisualEffect>();
        _Collider.enabled = false;
        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand
    }

    /// <summary>
    /// Adds the components of the element flyweight prefab to the currently blank projectile (texture, trails.. etc)
    /// </summary>
    public void InitElement(GameObject spellElementFlyweightObject)
    {
        //_visualEffect = GetComponent<VisualEffect>();


        //TODO: adjust VFX color instead



        //ComponentCopier.Copy(spellElementFlyweightObject, gameObject);
    }

    protected override void CastSpell()
    {
        _Collider.enabled = true;
        _visualEffect.Play();
    }

}

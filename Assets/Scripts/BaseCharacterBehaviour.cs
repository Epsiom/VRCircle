using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterBehaviour : MonoBehaviour
{
    [SerializeField] private int _Health = 1;

    public void DamageHealth(int damagePointsInflicted)
    {
        _Health -= damagePointsInflicted;
        if (_Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

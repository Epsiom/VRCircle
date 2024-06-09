using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private int _Health = 1;

    void Start()
    {
        // Adds the reference to this newly created enemy to the list of all enemies
        EnemyReferences.Instance.AddEnemyReference(this.gameObject);
    }

    public void DamageHealth(int damagePointsInflicted)
    {
        _Health -= damagePointsInflicted;
        if (_Health <= 0)
        {
            EnemyReferences.Instance.RemoveEnemyReference(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Called on contact with the player
    /// </summary>
    protected void DamagePlayer()
    {
        MainController.Instance.DamagePlayer();
        EnemyReferences.Instance.RemoveEnemyReference(this.gameObject);
        Destroy(this.gameObject);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The spell component added to the Ultracoin prefab
/// </summary>
public class UltracoinSpell : BaseSpellBehaviour
{
    [SerializeField] private GameObject _UltrashotPrefab;

    [SerializeField]
    [Tooltip("The layer of the environment")]
    private LayerMask _EnvironmentLayer;

    [SerializeField]
    [Tooltip("The layer of projectiles")]
    protected LayerMask _ProjectileLayer;

    [SerializeField] private float _CoinSpeed;
    [SerializeField] private float _CoinRotationSpeed = 20f;
    private Rigidbody _CoinRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        this._CoinRigidbody = this.transform.GetComponent<Rigidbody>();

        this.CastCountdown = 0.5f;
        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand until it fires

        this.transform.DORotate(new Vector3(180f, 0f, 0f), _CoinRotationSpeed, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
    }

    /// <summary>
    /// First off, toss a coin when the CastCountdown is elapsed.
    /// Then, instantiates the projectile prefab, that will fire by itself in its own spell component
    /// </summary>
    protected override void CastSpell()
    {
        this.transform.SetParent(null);

        Vector3 directionVector = (this.transform.position - GlobalReferences.Instance.Wand.position);   // The direction from the wand to the spell's position
        directionVector.Normalize();

        _CoinRigidbody.isKinematic = false;
        _CoinRigidbody.useGravity = true;
        _CoinRigidbody.velocity = directionVector * _CoinSpeed;

        // Instantiates the Ultrashot at its own
        GameObject spawnedUltrashot = Instantiate(_UltrashotPrefab, GlobalReferences.Instance.SpellSpawningPoint.position, GlobalReferences.Instance.SpellSpawningPoint.rotation);
    }

    /// <summary>
    /// When making contact with the environment, destroys the coin.
    /// When making contact with a projectile, redirects it toward the closest enemy
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.collider.gameObject.layer) & _EnvironmentLayer) != 0)
        {
            Destroy(this.gameObject);
            return;
        }
        if (((1 << collision.collider.gameObject.layer) & _ProjectileLayer) != 0)
        {
            // Redirects any projectile that hits the coin toward the closest enemy
            GameObject closestEnemy = EnemyReferences.Instance.FindClosestEnemyToPosition(this.transform);
            if (closestEnemy == null)
            {
                collision.gameObject.transform.LookAt(Vector3.down);
            }
            else
            {
                collision.gameObject.transform.LookAt(closestEnemy.transform);
            }

            // Destroys the coin
            Destroy(this.gameObject);
            return;
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The spell component added to the Ultracoin prefab.
/// Warning : it does not inherit from BaseSpellBehaviour
/// </summary>
public class UltracoinSpell : MonoBehaviour
{
    [SerializeField] private GameObject _UltrashotPrefab;

    [SerializeField]
    [Tooltip("The layer of the environment")]
    private LayerMask _EnvironmentLayer;

    [SerializeField]
    [Tooltip("The layer of projectiles")]
    private LayerMask _ProjectileLayer;

    [SerializeField] private float _CoinSpeed;
    //[SerializeField] private float _CoinRotationSpeed = 20f;
    private Rigidbody _CoinRigidbody;

    [SerializeField] protected float CoinThrowCountdownTimer = 0.5f; // Time in seconds after which the coin is thrown

    private bool IsCoinThrown = false;

    // Start is called before the first frame update
    private void Start()
    {
        this._CoinRigidbody = this.transform.GetComponent<Rigidbody>();

        Invoke(nameof(CastSpell), CoinThrowCountdownTimer);
        this.transform.SetParent(GlobalReferences.Instance.Wand.transform);    //Attaches the spell to the wand until it fires

        //this.transform.DORotate(new Vector3(180f, 0f, 0f), _CoinRotationSpeed, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
    }

    void Update()
    {
        if (IsCoinThrown)
        {
            transform.Rotate(1080 * Time.deltaTime, 0, 0); //rotates 1080 degrees per second around x axis
        }
    }

    /// <summary>
    /// First off, toss a coin when the CastCountdown is elapsed.
    /// Then, instantiates the projectile prefab, that will fire by itself in its own spell component
    /// </summary>
    private void CastSpell()
    {
        this.transform.SetParent(null);

        //Vector3 directionVector = (this.transform.position - GlobalReferences.Instance.Wand.position);   // The direction from the wand to the spell's position
        Vector3 directionVector = (this.transform.position - GlobalReferences.Instance.Wand.position);
        directionVector.y = 0;
        directionVector.Normalize();
        directionVector = Quaternion.AngleAxis(-45, new Vector3(directionVector.z, 0, -directionVector.x)) * directionVector;
        // or use result = Vector3.Lerp(up, facing, 0.5f).normalized;
        // or use result = Vector3.Slerp(up, facing, 0.5f);
        // or even var axis = Vector3.Cross(up, facing);
        //         result = Quaternion.AngleAxis(45, axis) * facing;

        _CoinRigidbody.isKinematic = false;
        _CoinRigidbody.useGravity = true;
        _CoinRigidbody.velocity = directionVector * _CoinSpeed;

        IsCoinThrown = true;

        // Instantiates the Ultrashot at its own
        GameObject spawnedUltrashot = Instantiate(_UltrashotPrefab, GlobalReferences.Instance.SpellSpawningPoint.position, GlobalReferences.Instance.SpellSpawningPoint.rotation);
    }

    /// <summary>
    /// When making contact with the environment, destroys the coin.
    /// When making contact with a projectile, redirects it toward the closest enemy
    /// </summary>
    /// <param name="collision"></param>
    protected void OnCollisionEnter(Collision collision)
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
            Vector3 directionVector;
            if (closestEnemy == null)
            {
                Debug.Log("No enemy found");
                //collision.gameObject.transform.LookAt(Vector3.down);
                directionVector = Random.onUnitSphere * _CoinSpeed;
                
            }
            else
            {
                Debug.Log("Enemy found");
                //collision.gameObject.transform.LookAt(closestEnemy.transform);
                directionVector = closestEnemy.transform.position - collision.gameObject.transform.position;
            }
            collision.gameObject.GetComponent<Rigidbody>().velocity = directionVector * _CoinSpeed;

            // Destroys the coin
            Destroy(this.gameObject);
            return;
        }
    }
}

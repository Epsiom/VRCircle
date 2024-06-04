using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _OuterRing;
    [SerializeField] private GameObject _InnerRing;

    [SerializeField] private GameObject _EnemyPrefabToSpawn;

    // Time in seconds after which an enemy is spawned
    [SerializeField] private float SpawnDelay = 5f;
    private float SpawnCountdown;

    // The path that all enemies will follow to reach the player
    [SerializeField] private Spline _PathToPlayer;
    [SerializeField] private float _SplineKnotPerDistanceUnit = 0.5f;       // Number of knots in the spline per Unity distance unit : a high value means a more twisted path
    [SerializeField] private float _SplineRandomnessMagnitude = 1f;         // The amount of randomness in the distance to stray from a direct path from the spawner to the player
    [SerializeField] private float _KnotWorldMinimalHeight = 1f;            // The minimal height of each spline knot in world coordinates, regardless of randomness

    // Update is called once per frame
    void Start()
    {
        _PathToPlayer = this.gameObject.GetComponent<SplineContainer>().Spline;
        SetPathToPlayer();

        Vector3 InnerRingRotation = new Vector3(180f, 0f, 0f);
        Vector3 OuterRingRotation = new Vector3(90f, 180f, 0f);
        _OuterRing.transform.DORotate(InnerRingRotation, 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        _InnerRing.transform.DORotate(OuterRingRotation, 3f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);

        SpawnCountdown = SpawnDelay;
    }

    void Update()
    {
        SpawnCountdown -= Time.deltaTime;
        if (SpawnCountdown <= 0)
        {
            GameObject spawnedEnemy = Instantiate(_EnemyPrefabToSpawn, this.transform.position, this.transform.rotation);
            SplineContainer splineContainer = this.gameObject.GetComponent<SplineContainer>();
            spawnedEnemy.GetComponent<SplineFollowingEnemyBehaviour>().Init(splineContainer);

            SpawnCountdown = SpawnDelay;
        }
    }

    /// <summary>
    /// Sets all nodes in the spline to create a twisted path from the spawner to the player
    /// (which is considered at (0,0,0) in the world, but -transform.position in the spawner's referential)
    /// </summary>
    private void SetPathToPlayer()
    {
        _PathToPlayer.Clear();
        float distanceFromSpawnerToPlayer = this.transform.position.magnitude;                          // Considering the player is at the origin, we only need the spawner's position
        float numberOfKnots = Mathf.Floor(distanceFromSpawnerToPlayer * _SplineKnotPerDistanceUnit);
        float minimalHeight = _KnotWorldMinimalHeight - transform.position.y;                           // Adjusts for the vertical offset of the spawner, considering the knot coordinates are relative to the spawner and not the world

        // First point
        _PathToPlayer.Add(new BezierKnot(new Vector3(0, 0, 0)));

        for (int knotNumber = 1; knotNumber < numberOfKnots; knotNumber++)
        {
            Vector3 pointOnThePath = (-this.transform.position / numberOfKnots) * knotNumber;
            Vector3 deviatedPoint = pointOnThePath + new Vector3(
                Random.Range(-_SplineRandomnessMagnitude, _SplineRandomnessMagnitude),
                Random.Range(-_SplineRandomnessMagnitude, _SplineRandomnessMagnitude),
                Random.Range(-_SplineRandomnessMagnitude, _SplineRandomnessMagnitude)
            );
            if (deviatedPoint.y <= minimalHeight) deviatedPoint.y = minimalHeight;                      // Ensures the knot never goes underneath the map
            BezierKnot bezierKnot = new BezierKnot(deviatedPoint);
            _PathToPlayer.Add(bezierKnot);
        }

        // Last point
        _PathToPlayer.Add(new BezierKnot(-this.transform.position));

        // Sets the tangent mode for all knots in the spline to "Auto Smooth"
        _PathToPlayer.SetTangentMode(TangentMode.AutoSmooth);
    }
}

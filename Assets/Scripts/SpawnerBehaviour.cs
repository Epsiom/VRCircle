using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _InnerRing;
    [SerializeField] private GameObject _OuterRing;

    [SerializeField] private GameObject _EnemyPrefabToSpawn;

    [SerializeField] private float SpawnDelay = 5f; // Time in seconds after which an enemy is spawned
    private float SpawnCountdown;

    // Update is called once per frame
    void Start()
    {
        Vector3 InnerRingRotation = new Vector3(180f, 0f, 0f);
        Vector3 OuterRingRotation = new Vector3(90f, 180f, 0f);
        _InnerRing.transform.DORotate(InnerRingRotation, 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        _OuterRing.transform.DORotate(OuterRingRotation, 3f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);

        SpawnCountdown = SpawnDelay;
    }

    void Update()
    {
        SpawnCountdown -= Time.deltaTime;
        if (SpawnCountdown <= 0)
        {
            Instantiate(_EnemyPrefabToSpawn, this.transform.position, this.transform.rotation);
            SpawnCountdown = SpawnDelay;
        }
    }
}

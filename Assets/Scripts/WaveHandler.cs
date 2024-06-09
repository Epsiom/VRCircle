using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    // Timer variables
    private const float WAVE_DURATION = 15f; 
    private float _WaveCountdownTimer = 0f;

    // Wave variables
    //private float RemainingWaveTime;
    private bool _IsWaveInProgress = false;
    private List<SpawnerBehaviour> spawnerBehaviourList = new List<SpawnerBehaviour>();

    // Spawner procedural instantiation variables
    private const int SPAWNER_AMOUNT = 3;
    private float _SpawnerMaximumAngleToFrontOfOrigin = 90f;    // The instantiation angle in front of the player's origin, in degree
    private float _SpawnerMinimumDistanceToOrigin = 10f;
    private float _SpawnerMaximumDistanceToOrigin = 35f;
    private float _SpawnerMinimumHeight = 5f;
    private float _SpawnerMaximumHeight = 10f;
    private float _SpawnerMinimumSpacing = 10f;                 // The minimum distance to respect between the origin of two spawners


    public void StartNewWave()
    {
        if (_IsWaveInProgress) return;

        List<Vector3> spawnerPositionList = ComputeSpawnerPositionList(SPAWNER_AMOUNT);
        foreach (Vector3 existingSpawnerPosition in spawnerPositionList)
        {
            GameObject spawnPrefab = EnemyReferences.Instance.SpawnerPrefab;
            GameObject spawner = Object.Instantiate(spawnPrefab, existingSpawnerPosition, Quaternion.identity);
            spawnerBehaviourList.Add(spawner.GetComponent<SpawnerBehaviour>());
        }
        _IsWaveInProgress = true;
        _WaveCountdownTimer = WAVE_DURATION;
    }

    void Update()
    {
        // Handles the wave time
        if (_IsWaveInProgress)
        {
            _WaveCountdownTimer -= Time.deltaTime;
            if (_WaveCountdownTimer <= 0f)
            {
                EndSpawnWave();
            }
            GlobalReferences.Instance.WaveTimerTMP.text = System.Math.Round(_WaveCountdownTimer, 2).ToString();
        }
    }

    public void EndSpawnWave()
    {
        _WaveCountdownTimer = 0f;
        _IsWaveInProgress = false;

        // Removes all spawners
        foreach (SpawnerBehaviour spawner in spawnerBehaviourList)
        {
            spawner.DisableSpawner();
        }
    }

    public void LastEnemyKilled()
    {
        if (!_IsWaveInProgress)
        {
            foreach (SpawnerBehaviour spawnerBehaviour in spawnerBehaviourList)
            {
                Object.Destroy(spawnerBehaviour.gameObject);
            }
        }
    }

    /// <summary>
    /// Returns a list of correct positions to instantiate the spawners
    /// </summary>
    private List<Vector3> ComputeSpawnerPositionList(int spawnerAmount)
    {
        List<Vector3> spawnerPositionList = new List<Vector3>();
        while (spawnerPositionList.Count < spawnerAmount)
        {
            Quaternion deviationRotation = Quaternion.Euler(0, Random.Range(-_SpawnerMaximumAngleToFrontOfOrigin, _SpawnerMaximumAngleToFrontOfOrigin), 0);
            Vector3 spawnerPosition = deviationRotation * Vector3.forward * Random.Range(_SpawnerMinimumDistanceToOrigin, _SpawnerMaximumDistanceToOrigin);
            float spawnerHeight = Random.Range(_SpawnerMinimumHeight, _SpawnerMaximumHeight);
            spawnerPosition += Vector3.up * spawnerHeight;
            if (IsSpawnerPositionTooCloseToOthers(spawnerPosition, spawnerPositionList))
            {
                continue;   // Ignores that attempt, and loop back to generate more positions until we have spawnerAmount
            }
            else
            {
                spawnerPositionList.Add(spawnerPosition);   // Adds the spawner position to the list
            }
        }
        return spawnerPositionList;
    }

    private bool IsSpawnerPositionTooCloseToOthers(Vector3 spawnerPosition, List<Vector3> spawnerPositionList)
    {
        foreach (Vector3 existingSpawnerPosition in spawnerPositionList)
        {
            if (Vector3.Distance(existingSpawnerPosition, spawnerPosition) <= _SpawnerMinimumSpacing)
            {
                return true;
            }
        }
        return false;
    }


    // --- Singleton management ---

    private static WaveHandler instance;

    // This is the public reference to the instance
    public static WaveHandler Instance
    {
        get
        {
            // If the instance is null, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<WaveHandler>();

                // If it's still null, create a new GameObject and add the script
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(WaveHandler).Name);
                    instance = singletonObject.AddComponent<WaveHandler>();
                }
            }
            return instance;
        }
    }

    // Makes sure to prevent duplicates of the Singleton by destroying them
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

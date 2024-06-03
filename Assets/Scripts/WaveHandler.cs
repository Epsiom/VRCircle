using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler
{
    // Wave variables
    //private float RemainingWaveTime;
    private List<SpawnerBehaviour> spawnerBehaviourList = new List<SpawnerBehaviour>();

    // Spawner procedural instantiation variables
    private const int SPAWNER_AMOUNT = 3;
    private float _SpawnerMaximumAngleToFrontOfOrigin = 90f;    // The instantiation angle in front of the player's origin, in degree
    private float _SpawnerMinimumDistanceToOrigin = 10f;
    private float _SpawnerMaximumDistanceToOrigin = 15f;
    private float _SpawnerMinimumSpacing = 10f;                 // The minimum distance to respect between the origin of two spawners

    public void StartNewWave()
    {
        List<Vector3> spawnerPositionList = ComputeSpawnerPositionList(SPAWNER_AMOUNT);
        foreach (Vector3 existingSpawnerPosition in spawnerPositionList)
        {
            GameObject enemyPrefab = EnemyReferences.Instance.DefaultEnemy;
            GameObject spawner = Object.Instantiate(enemyPrefab, existingSpawnerPosition, Quaternion.identity);
            spawnerBehaviourList.Add(spawner.GetComponent<SpawnerBehaviour>());
        }
    }


    /// <summary>
    /// Returns a list of 
    /// </summary>
    /// <param name="spawnerAmount"></param>
    /// <returns></returns>
    private List<Vector3> ComputeSpawnerPositionList(int spawnerAmount)
    {
        List<Vector3> spawnerPositionList = new List<Vector3>();
        while (spawnerPositionList.Count < spawnerAmount)
        {
            Quaternion deviationRotation = Quaternion.Euler(0, Random.Range(-_SpawnerMaximumAngleToFrontOfOrigin, _SpawnerMaximumAngleToFrontOfOrigin), 0);
            Vector3 spawnerPosition = deviationRotation * Vector3.forward * Random.Range(_SpawnerMinimumDistanceToOrigin, _SpawnerMaximumDistanceToOrigin);
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
}

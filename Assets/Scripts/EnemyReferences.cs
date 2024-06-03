using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a reference to all enemy GameObjects
/// </summary>
public class EnemyReferences : MonoBehaviour
{
    [Header("ENEMY PREFABS:")]
    [Tooltip("The default enemy")]
    public GameObject DefaultEnemy;

    [Header("ENEMY INSTANCE LIST:")]
    [Tooltip("The list of all currently alive enemies")]
    public List<GameObject> Enemies = new List<GameObject>();

    /// <summary>
    /// Stores the reference of a newly created GameObject in the list
    /// </summary>
    /// <param name="enemyGameobject"></param>
    public void AddEnemyReference(GameObject enemyGameobject)
    {
        // Avoids duplicates
        if (Enemies.IndexOf(enemyGameobject) == -1)
        {
            Enemies.Add(enemyGameobject);
        }
    }

    public void RemoveEnemyReference(GameObject enemyGameobject)
    {
        Enemies.Remove(enemyGameobject);
    }

    /// <summary>
    /// Returns either the closest enemy to the target transform, or null if there's no enemies
    /// </summary>
    /// <param name="targetTransform">The transform of the GameObject we want to find the closest enemy of</param>
    /// <returns>Either the GameObject of the closest enemy, or null if there are no enemies</returns>
    public GameObject FindClosestEnemyToPosition(Transform targetTransform)
    {
        if (this.Enemies.Count == 0) return null;
        
        GameObject closestEnemy = Enemies[0];
        float closestEnemyDistance = float.PositiveInfinity;

        foreach (GameObject enemy in this.Enemies)
        {
            float enemyDistance = Vector3.Distance(enemy.transform.position, targetTransform.position);
            if (enemyDistance < closestEnemyDistance)
            {
                closestEnemy = enemy;
                closestEnemyDistance = enemyDistance;
            }
        }
        return closestEnemy;
    }


    // --- Singleton management ---

    private static EnemyReferences instance;

    // This is the public reference to the instance
    public static EnemyReferences Instance
    {
        get
        {
            // If the instance is null, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyReferences>();

                // If it's still null, create a new GameObject and add the script
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(EnemyReferences).Name);
                    instance = singletonObject.AddComponent<EnemyReferences>();
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

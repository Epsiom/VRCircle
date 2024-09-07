using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The center of the flock moving along the spline
// In charge of generating the enemies with FlockEnemyBehaviour, and to keep a pointer to all of them
public class FlockCenterBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _FlockEnemyPrefab;

    private List<FlockEnemyBehaviour> _boids = new List<FlockEnemyBehaviour>();

    private const int FLOCK_ENEMY_COUNT = 8;
    private const float MAX_DISTANCE_TO_CENTER = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiates flock enemies at equal distance around the center
        for (int i=0; i<FLOCK_ENEMY_COUNT; i++)
        {
            Quaternion rotationAroundCenter = Quaternion.Euler(i * 360 / FLOCK_ENEMY_COUNT, 0, 0);
            Vector3 boidPosition = this.transform.position + rotationAroundCenter * (MAX_DISTANCE_TO_CENTER * Vector3.forward);
            GameObject boid = Instantiate(_FlockEnemyPrefab, boidPosition, Random.rotation, this.transform);

            FlockEnemyBehaviour boidFlockEnemyBehaviour = boid.GetComponent<FlockEnemyBehaviour>();
            //boidFlockEnemyBehaviour.Init(this, _boids);

            _boids.Add(boidFlockEnemyBehaviour);
        }
    }
}

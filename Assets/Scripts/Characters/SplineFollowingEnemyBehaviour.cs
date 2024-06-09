using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowingEnemyBehaviour : BaseEnemyBehaviour
{
    private float followSpeed = 2f;
    private SplineAnimate _splineAnimate;

    private const float DISTANCE_TO_ORIGIN_TO_DAMAGE_PLAYER = 1.5f;   // Or use _splineAnimate.NormalizedTime instead (0-1 progression along the spline)

    public void Init(SplineContainer splineContainer)
    {
        //transform.DOMove(playerObject.transform.position, followSpeed).SetLoops(-1, LoopType.Restart);
        _splineAnimate = this.gameObject.AddComponent<SplineAnimate>();
        _splineAnimate.Container = splineContainer;
        _splineAnimate.Alignment = SplineAnimate.AlignmentMode.SplineElement;
        _splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
        _splineAnimate.MaxSpeed = followSpeed;
        _splineAnimate.Loop = SplineAnimate.LoopMode.Once;
    }

    void Update()
    {
        //this.transform.LookAt(GlobalReferences.Instance.Player.position);

        float distanceToOrigin = Vector3.Distance(transform.position, Vector3.zero);    // No need to take the distance to the player, we take the distance to the center of the arena instead
        if (distanceToOrigin < DISTANCE_TO_ORIGIN_TO_DAMAGE_PLAYER)
        {
            DamagePlayer();
        }
    }
}

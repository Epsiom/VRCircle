using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowingEnemyBehaviour : BaseEnemyBehaviour
{
    private float followSpeed = 2f;

    public void Init(SplineContainer splineContainer)
    {
        //transform.DOMove(playerObject.transform.position, followSpeed).SetLoops(-1, LoopType.Restart);
        SplineAnimate splineAnimate = this.gameObject.AddComponent<SplineAnimate>();
        splineAnimate.Container = splineContainer;
        splineAnimate.Alignment = SplineAnimate.AlignmentMode.SplineElement;
        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
        splineAnimate.MaxSpeed = followSpeed;
        splineAnimate.Loop = SplineAnimate.LoopMode.Once;
    }

    /*
    void Update()
    {
        this.transform.LookAt(GlobalReferences.Instance.Player.position);
    }*/
}

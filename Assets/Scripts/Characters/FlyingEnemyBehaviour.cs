using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class FlyingEnemyBehaviour : BaseCharacterBehaviour
{
    private float followSpeed = 0.005f;

    void Start()
    {
        //transform.DOMove(playerObject.transform.position, followSpeed).SetLoops(-1, LoopType.Restart);
    }

    void Update()
    {
        Vector3 directionVector = (GlobalReferences.Instance.Player.position - this.transform.position);
        directionVector.Normalize();
        this.transform.position += directionVector * followSpeed;

        this.transform.LookAt(GlobalReferences.Instance.Player.position);
    }
}

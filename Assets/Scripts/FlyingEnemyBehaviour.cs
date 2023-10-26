using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class FlyingEnemyBehaviour : MonoBehaviour
{
    private GameObject playerObject;
    private float followSpeed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("XR Origin (XR Rig)");
        //transform.DOMove(playerObject.transform.position, followSpeed).SetLoops(-1, LoopType.Restart);
    }

    void Update()
    {
        Vector3 directionVector = (playerObject.transform.position - this.transform.position);
        directionVector.Normalize();
        this.transform.position += directionVector * followSpeed;

        this.transform.LookAt(playerObject.transform.position);
    }
}

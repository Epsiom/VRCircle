using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardBehaviour : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(GlobalReferences.Instance.Player);
    }
}

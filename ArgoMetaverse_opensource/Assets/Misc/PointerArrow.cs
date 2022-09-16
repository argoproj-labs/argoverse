using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    [SerializeField] Transform targetToLook, targetToPoint;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation((targetToLook.position - transform.position), -(targetToPoint.position - transform.position));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private Vector3 degreesPerSec;

    private void FixedUpdate() => transform.Rotate(degreesPerSec * Time.fixedDeltaTime, Space.Self);
}

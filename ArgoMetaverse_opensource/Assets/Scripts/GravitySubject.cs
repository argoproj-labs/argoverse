using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravitySubject : MonoBehaviour
{
    private Rigidbody _rb;
    private float underwaterModifier = 1.0f;
    private void Awake() => _rb = GetComponent<Rigidbody>();
    private void FixedUpdate() => _rb.AddForce((GravitySource.Instance.transform.position - transform.position).normalized * GravitySource.Instance.Gravity * underwaterModifier, ForceMode.Acceleration);

    public void Underwater(bool isUnderwater)
    {
        if (isUnderwater)
            underwaterModifier = 0.3f;
        else
            underwaterModifier = 1.0f;
    }
}

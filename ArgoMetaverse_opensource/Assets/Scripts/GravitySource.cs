using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public static GravitySource Instance; //its a singleton, gravitysubject needs to be refactored as well in a scenario with more sources.

    public float _gravityForce = 9.7f;
    public float Gravity => _gravityForce;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

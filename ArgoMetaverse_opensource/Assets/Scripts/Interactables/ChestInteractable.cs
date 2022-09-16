using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : InteractableObject
{
    private Animator _animator;
    private float _timestamp = -5f;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void CLickOnThis()
    {
        if (_timestamp <= Time.time - 5f)
        {
            _timestamp = Time.time;
            _animator.SetTrigger("ChestOpen");
        }
    }

}

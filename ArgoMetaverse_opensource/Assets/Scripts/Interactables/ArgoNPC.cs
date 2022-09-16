using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AlmenaraGames;

public class ArgoNPC : InteractableObject
{
    public bool disabled;

    private Transform targetToLookAt;

    public Animator _animator;
    //[SerializeField] private AudioObject AO_idle, AO_float, AO_jump, AO_lookaround, AO_spin, AO_wave, AO_salute;
    //[SerializeField] private MultiAudioSource audioSource;
    [SerializeField] private Transform eyeL, eyeR;

    private Camera _mainCam;
    public bool wasClicked { get; private set; } = false;
    protected override void Awake()
    {
        if (disabled)
            return;
        base.Awake();
        _animator = GetComponent<Animator>();

    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    public void Poke()
    {
        if (disabled)
            return;
        if (wasClicked)
            return;
        wasClicked = true;
        _animator.SetTrigger("hello");
    }

    private void IdleEnd()
    {
        if (disabled)
            return;
        if (wasClicked)
            return;
        float rNum = Random.Range(0f, 1f);
        if (rNum <= 0.25f)
            _animator.SetTrigger("idle1");
        else if (rNum <= 0.5f)
            _animator.SetTrigger("idle2");
    }

    private void WasClickedEnd()
    {
        if (disabled)
            return;
        wasClicked = false;
    }

    public void PlaySound(int index)
    {
        if (disabled)
            return;
        switch (index)
        {
            //case 0:
            //    AO_idle?.Play(2, transform.position);
            //    break;
            //case 1:
            //    AO_float?.Play(2, transform.position);
            //    break;
            //case 2:
            //    AO_jump?.Play(2, transform.position);
            //    break;
            //case 3:
            //    AO_lookaround?.Play(2, transform.position);
            //    break;
            //case 4:
            //    AO_spin?.Play(2, transform.position);
            //    break;
            //case 5:
            //    AO_wave?.Play(2, transform.position);
            //    break;
            //case 6:
            //    AO_salute?.Play(2, transform.position);
            //    break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        LookAtTarget();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            targetToLookAt = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            targetToLookAt = null;
    }

    private void LookAtTarget()
    {
        if (targetToLookAt == null)
            return;
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(targetToLookAt.position - transform.position, transform.up)), 0.08f);
        eyeR.rotation = Quaternion.LookRotation(-eyeR.forward, (Camera.main.transform.position - eyeR.position).normalized);
        eyeL.rotation = Quaternion.LookRotation(eyeL.forward, (Camera.main.transform.position - eyeL.position).normalized);
    }
}

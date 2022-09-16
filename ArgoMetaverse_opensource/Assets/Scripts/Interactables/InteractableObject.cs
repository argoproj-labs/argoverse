using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using AlmenaraGames;

public class InteractableObject : MonoBehaviour
{
    public enum InteractableType { Pickable, NPC, BoardClick, Other }

    public InteractableType type;

    public Rigidbody rb { get; private set; }
    public Collider col { get; private set; }

    [HideInInspector] public bool inactive = false;
    private Vector3 initialPos;
    private Quaternion initialRot;

    //private MultiAudioSource audioSource;

    public ItemNoises itemHitNoise;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        //audioSource = GetComponent<MultiAudioSource>();
        itemHitNoise = GetComponent<ItemNoises>();
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    public void ResetPositionRotation()
    {
        if (rb != null)
            rb.velocity = Vector3.zero;
        transform.SetParent(null);
        transform.position = initialPos;
        transform.rotation = initialRot;
    }
    
    public void PlaySound()
    {
        //if (audioSource != null)
        //    audioSource.Play();
    }

    public bool IsGrounded()
    {
        if (itemHitNoise != null)
        {
            return itemHitNoise.isGrounded;
        }
        else
            return true;
    }

    public virtual void CLickOnThis()
    {

    }
}

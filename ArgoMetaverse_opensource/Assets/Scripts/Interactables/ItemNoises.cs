using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AlmenaraGames;

public class ItemNoises : MonoBehaviour
{
    //This class is not fully implemented because the grab/throw mechanic was discontinued. But things are almost ready:
    //- link sounds via multiaudiomanager (uncomment the almenaragames dependancy)
    //- attribute the correct layer numbers on the switch case inside oncolision enter, being 1 = wall, 2 = ground and 3 = player/NPC

    public enum ItemMaterial { metal, wood, plastic, rock, generic }
    [SerializeField] private ItemMaterial itemMaterial;
    public bool isGrounded { get; private set; } = true;
    private float lastTimePlayed = 0;
    const float cooldown = 0.2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.timeSinceLevelLoad < 2f)
            return;
        switch (collision.gameObject.layer)
        {
            case 1: //change to wall layer #
                PlayHitGround();
                break;
            case 2: //change to ground layer #
                PlayHitGround();
                isGrounded = true;
                break;
            case 3: //change to NPC layer #
                if (!isGrounded)
                    PlayHitNPC();
                break;
        }
    }

    public void Grab()
    {
        isGrounded = false;
    }

    private void PlayHitGround()
    {
        if (lastTimePlayed > Time.time - cooldown)
            return;
        lastTimePlayed = Time.time;
        switch (itemMaterial)
        {
            case ItemMaterial.metal:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.wood:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.plastic:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.rock:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.generic:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            default:
                break;
        }
    }

    private void PlayHitNPC()
    {
        if (lastTimePlayed > Time.time - cooldown)
            return;
        lastTimePlayed = Time.time;
        switch (itemMaterial)
        {
            case ItemMaterial.metal:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.wood:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.plastic:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.rock:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            case ItemMaterial.generic:
                //MultiAudioManager.PlayAudioObjectByIdentifier("", transform);
                break;
            default:
                break;
        }
    }
}

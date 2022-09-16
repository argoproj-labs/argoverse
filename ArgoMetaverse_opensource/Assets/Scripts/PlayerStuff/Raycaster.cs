using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private Transform cameraTransform;
    public bool isOn { get; private set; } = false;
    private Player owner;
    private RaycastHit rayHit;
    public GameObject lastObjectHit;



    #region UnityMethods
    private void Update()
    {
        if (!isOn)
            return;
        FireRaycast();
    }
    #endregion

    public void InitializeRaycaster(Player ownerPlayer, Transform camTr)
    {
        owner = ownerPlayer;
        cameraTransform = camTr;
        isOn = true;
    }

    private void FireRaycast()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rayHit, 100f, 192) && rayHit.collider.gameObject.layer == 6)
        {
            if (lastObjectHit == null || lastObjectHit != rayHit.collider.gameObject)
            {
                lastObjectHit = rayHit.collider.gameObject;
                owner.AimAtInteractableObject(lastObjectHit);
            }
        }
        else
        {
            if (lastObjectHit != null)
            {
                lastObjectHit = null;
                owner.ClearTargetedInteractable();
            }
        }
    }
}

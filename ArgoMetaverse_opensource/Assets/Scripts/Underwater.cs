using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out HeadPlayerConnector p))
            return;

        p.Underwater(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out HeadPlayerConnector p))
            return;

        p.Underwater(false);
    }
}

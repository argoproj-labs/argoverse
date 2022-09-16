using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPlayerConnector : MonoBehaviour
{
    public Player _player;

    public void Underwater (bool isUnder)
    {
        _player.Underwater(isUnder);
    }
}

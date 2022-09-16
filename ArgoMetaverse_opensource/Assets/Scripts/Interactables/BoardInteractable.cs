using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInteractable : InteractableObject
{
    public Texture _sprite;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void CLickOnThis()
    {
        if(_sprite == null)
        {
            Debug.LogError("no sprite attributed to this board interactable trigger");
            return;
        }
        BoardPopup.Instance.ClickOnThis(transform, _sprite);
    }

}

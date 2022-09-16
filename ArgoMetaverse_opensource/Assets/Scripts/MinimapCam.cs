using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = Player.Instance.transform;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _playerTransform.position, transform.up);
    }
}

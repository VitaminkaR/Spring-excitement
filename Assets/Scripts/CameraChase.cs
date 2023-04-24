using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChase : MonoBehaviour
{
    [SerializeField] private GameObject _chaseObj;
    [SerializeField] private Vector3 _basePos;
    [SerializeField] private float _smooth;

    private void Start()
    {
        _chaseObj = BaseManager.Manager.Player.gameObject;
    }

    void Update()
    {
        Vector3 positionToGo = _chaseObj.transform.position + _basePos;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, positionToGo, _smooth * Time.deltaTime);
        transform.position = smoothPosition;
        transform.LookAt(_chaseObj.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SnowballAttack : MonoBehaviour
{
    private Enemy _isSeePlayer;
    [SerializeField] private int _timeReload;

    void Start()
    {
        
    }

    void Update()
    {
        if (_isSeePlayer._seePlayer == true)
        {
            StartCoroutine(Pause());
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(_timeReload);
    }
}

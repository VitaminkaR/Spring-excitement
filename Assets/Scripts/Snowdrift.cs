using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowdrift : MonoBehaviour
{
    // насколько сугроб будет замедлять игрока
    [SerializeField] private float _speedMultiplier;

    private float _startSpeed = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _startSpeed == 0 && !other.isTrigger)
        {
            _startSpeed = other.gameObject.GetComponent<Player>().Speed;
            other.gameObject.GetComponent<Player>().Speed = _startSpeed / _speedMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _startSpeed != 0 && !other.isTrigger)
        {
            other.gameObject.GetComponent<Player>().Speed = _startSpeed;
            _startSpeed = 0;
        }
    }
}

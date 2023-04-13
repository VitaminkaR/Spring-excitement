using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowdrift : MonoBehaviour
{
    // насколько сугроб будет замедлять игрока
    [SerializeField] private float _speedMultiplier;

    [SerializeField] private float _shelterSpawningChance;

    private float _startSpeed = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _startSpeed == 0 && !other.isTrigger && !other.gameObject.GetComponent<Player>().InSnowDrift)
        {
            _startSpeed = other.gameObject.GetComponent<Player>().Speed;
            other.gameObject.GetComponent<Player>().Speed = _startSpeed / _speedMultiplier;
            other.gameObject.GetComponent<Player>().InSnowDrift = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _startSpeed != 0 && !other.isTrigger)
        {
            other.gameObject.GetComponent<Player>().InSnowDrift = false;
            other.gameObject.GetComponent<Player>().Speed = _startSpeed;
            _startSpeed = 0;
        }
    }
}

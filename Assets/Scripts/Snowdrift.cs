using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snowdrift : MonoBehaviour
{
    // ��������� ������ ����� ��������� ������
    [SerializeField] private float _speedMultiplier;

    // shelter
    // ���� ������
    [SerializeField] private float _shelterSpawningChance;
    // ���� �� �� � ������ �������
    [SerializeField] private bool _isShelter;
    // ���� �������
    [SerializeField] private int _shelterDamage;
    // ������ ps
    [SerializeField] private GameObject _particleSystemPrefab;
    // particle system
    private GameObject _particleSystem;

    private float _startSpeed = 0;

    private void Start()
    {
        int chance = Random.Range(0, 100);
        if(_shelterSpawningChance > chance)
            _isShelter = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _startSpeed == 0 && !other.isTrigger && !other.gameObject.GetComponent<Player>().InSnowDrift)
        {
            _startSpeed = other.gameObject.GetComponent<Player>().Speed;
            other.gameObject.GetComponent<Player>().Speed = _startSpeed / _speedMultiplier;
            other.gameObject.GetComponent<Player>().InSnowDrift = true;

            if(_isShelter)
            {
                _particleSystem = Instantiate(_particleSystemPrefab, transform);
                other.gameObject.GetComponent<Player>().Health -= _shelterDamage;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _startSpeed != 0 && !other.isTrigger)
        {
            other.gameObject.GetComponent<Player>().InSnowDrift = false;
            other.gameObject.GetComponent<Player>().Speed = _startSpeed;
            _startSpeed = 0;

            Destroy(_particleSystem);
        }
    }
}

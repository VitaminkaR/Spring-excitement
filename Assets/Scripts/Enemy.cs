using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;

    [SerializeField] public Player _player;
    private NavMeshAgent _navigationAgent;

    // расстояние на котором игрок находится в пределах видимости (sphere collider radius * 2)
    private float _viewDistance;
    // виден ли игрок (прямая видимость)
    [HideInInspector] public bool _isVisible;
    // расстояние от игрока на котором будут останавливаться противники
    [SerializeField] private float _stopDistance;

    // урон от атаки ближнего боя
    [SerializeField] private int _nearAttackDamage;
    [SerializeField] private float _reloadNA;
    private float _timerNA;

    private void Start()
    {
        _navigationAgent = GetComponent<NavMeshAgent>();
        _viewDistance = GetComponent<SphereCollider>().radius * 2;
    }

    private void Update()
    {
        if (_player != null && _isVisible)
        {
            transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
        }
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            // проверка на видимость
            Vector3 vec = _player.transform.position - transform.position;
            float distance = vec.magnitude;
            if (distance < _viewDistance)
            {
                // нанесение урона
                if (_timerNA < 0 && distance < 2)
                {
                    _player.GetComponent<Player>().Health -= _nearAttackDamage;
                    _timerNA = _reloadNA;
                }
                else
                {
                    _timerNA -= Time.deltaTime;
                }
                    

                // проверка есть ли прямая видимость
                Vector3 dir = vec / distance;
                RaycastHit hit;
                Debug.DrawRay(transform.position, dir * _viewDistance, Color.red);
                if (Physics.Raycast(transform.position, dir * _viewDistance, out hit) && hit.collider.gameObject.CompareTag("Player"))
                    _isVisible = true;
                else
                    _isVisible = false;

                // назначение движения к игроку
                if (_isVisible)
                    _navigationAgent.destination = _player.transform.position;

                // остановка перед игроком
                if (distance < _stopDistance)
                    _navigationAgent.Stop();
                else
                    _navigationAgent.Resume();
            }
            else
            {
                _isVisible = false;
            }
        }
    }

    // зона просмотра врагов
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _player = other.gameObject.GetComponent<Player>();
    }

    // наносит урон врагу
    public void Damage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Death();
    }

    // смерть врага
    private void Death()
    {
        Destroy(gameObject);
    }
}

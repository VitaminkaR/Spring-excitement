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
    private Rigidbody _rigidbody;
    private NavMeshAgent _navigationAgent;

    // расстояние на котором игрок находится в пределах видимости (sphere collider radius * 2)
    private float _viewDistance;
    // виден ли игрок (прямая видимость)
    [HideInInspector] public bool _isVisible;
    // расстояние от игрока на котором будут останавливаться противники
    [SerializeField] private float _stopDistance;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _navigationAgent = GetComponent<NavMeshAgent>();
        _viewDistance = GetComponent<SphereCollider>().radius * 2;
    }

    private void Update()
    {
        if (_player != null && _isVisible)
        {
            transform.LookAt(_player.transform.position);
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
        if (_health < 0)
            Death();
    }

    // наносит урон врагу и отталкивает его
    public void Damage(float damage, Vector3 force)
    {
        _health -= damage;
        if (_health < 0)
            Death();

        _rigidbody.AddForce(force);
    }

    // смерть врага
    private void Death()
    {
        Destroy(gameObject);
    }
}

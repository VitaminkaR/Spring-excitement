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

    // расстояние на котором игрок находится в пределах видимости
    [SerializeField] private float _viewDistance;
    // виден ли игрок (прямая видимость)
    [HideInInspector] public bool _isVisible;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _navigationAgent = GetComponent<NavMeshAgent>();
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
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            if (distance < _viewDistance)
                _isVisible = true;
            else
                _isVisible = false;

            if(_isVisible)
            {
                _navigationAgent.destination = _player.transform.position;
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
        if(_health < 0)
            Death();
    }

    // наносит урон врагу и отталкивает его
    public void Damage(float damage, Vector3 force)
    {
        _health -= damage;
        if (_health < 0)
            Death();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if(rigidbody != null)
        {
            rigidbody.AddForce(force);
        }
    }

    // смерть врага
    private void Death()
    {
        Destroy(gameObject);
    }
}

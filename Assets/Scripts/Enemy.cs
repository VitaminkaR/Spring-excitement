using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;

    [SerializeField] private Player _player;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _viewDistance;
    [HideInInspector] public bool _seePlayer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_player != null && _seePlayer)
        {
            transform.LookAt(_player.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            if (distance < _viewDistance)
            {
                _seePlayer = true;
            }
            else
            {
                _seePlayer = false;
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

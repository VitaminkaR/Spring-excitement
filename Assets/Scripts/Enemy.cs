using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;

    [SerializeField] public Player _player;
    private NavMeshAgent _navigationAgent;
    private Rigidbody _rigidbody;

    // расстояние на котором игрок находится в пределах видимости (sphere collider radius * 2)
    private float _viewDistance;
    // виден ли игрок (прямая видимость)
    [HideInInspector] public bool _isVisible;
    // расстояние от игрока на котором будут останавливаться противники
    [SerializeField] private float _stopDistance;

    // время стана
    [SerializeField] private float _stunTime;
    public bool IsStunned;

    // урон от атаки ближнего боя
    [SerializeField] private int _nearAttackDamage;
    [SerializeField] private float _reloadNA;
    private float _timerNA;

    private void Start()
    {
        _navigationAgent = GetComponent<NavMeshAgent>();
        _viewDistance = GetComponent<SphereCollider>().radius * 2;
        _rigidbody = GetComponent<Rigidbody>();
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
                // проверка есть ли прямая видимость
                Vector3 dir = vec / distance;
                RaycastHit hit;
                Debug.DrawRay(transform.position, dir * _viewDistance, Color.red);
                if (Physics.Raycast(transform.position, dir * _viewDistance, out hit) && hit.collider.gameObject.CompareTag("Player") && !IsStunned)
                    _isVisible = true;
                else
                    _isVisible = false;

                if(!IsStunned)
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

    // наносит урон врагу, отталкивает и глушит врага
    public void Damage(float damage, Vector3 force)
    {
        _health -= damage;
        if (_health <= 0)
            Death();

        if (IsStunned)
            StopCoroutine(Stun(force));
        StartCoroutine(Stun(force));
    }

    IEnumerator Stun(Vector3 force)
    {
        IsStunned = true;
        _navigationAgent.enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(force, ForceMode.Impulse);

        yield return new WaitForSeconds(_stunTime);
        IsStunned = false;
        _navigationAgent.enabled = true;
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    // смерть врага
    private void Death()
    {
        Destroy(gameObject);
    }
}

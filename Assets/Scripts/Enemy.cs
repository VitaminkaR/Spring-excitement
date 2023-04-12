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

    // ���������� �� ������� ����� ��������� � �������� ��������� (sphere collider radius * 2)
    private float _viewDistance;
    // ����� �� ����� (������ ���������)
    [HideInInspector] public bool _isVisible;

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
            // �������� �� ���������
            Vector3 vec = _player.transform.position - transform.position;
            float distance = vec.magnitude;
            if (distance < _viewDistance)
            {
                Vector3 dir = vec / distance;
                RaycastHit hit;
                Debug.DrawRay(transform.position, dir * _viewDistance, Color.red);
                if (Physics.Raycast(transform.position, dir * _viewDistance, out hit) && hit.collider.gameObject.CompareTag("Player"))
                    _isVisible = true;
                else
                    _isVisible = false;

                if (_isVisible)
                    _navigationAgent.destination = _player.transform.position;
            }
        }
    }

    // ���� ��������� ������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _player = other.gameObject.GetComponent<Player>();
    }

    // ������� ���� �����
    public void Damage(float damage)
    {
        _health -= damage;
        if (_health < 0)
            Death();
    }

    // ������� ���� ����� � ����������� ���
    public void Damage(float damage, Vector3 force)
    {
        _health -= damage;
        if (_health < 0)
            Death();

        _rigidbody.AddForce(force);
    }

    // ������ �����
    private void Death()
    {
        Destroy(gameObject);
    }
}

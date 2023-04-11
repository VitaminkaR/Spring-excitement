using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;


    // ������� ���� �����
    public void Damage(float damage)
    {
        _health -= damage;
        if(_health < 0)
            Death();
    }

    // ������� ���� ����� � ����������� ���
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

    // ������ �����
    public void Death()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _speed;

    // ������ �������
    [SerializeField] private float _punchDamage;
    // ����� ������� ��������� � ������� �������� �����
    [SerializeField] private List<Enemy> _enemies;



    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // ��������
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * _speed);

        // �������
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Quaternion deg = Quaternion.Euler(0, Mathf.Atan2(-y, x) * Mathf.Rad2Deg, 0);
        if (x != 0 || y != 0)
            _rigidbody.MoveRotation(deg);

        // ������� �����
        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    // ������� ����� ����� ��������� � ���� �������� �����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            _enemies.Add(other.gameObject.GetComponent<Enemy>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            _enemies.Remove(other.gameObject.GetComponent<Enemy>());
    }


    // ������� �����
    void Attack()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            // ��������� �����
           //_enemies[i].
        }
    }
}

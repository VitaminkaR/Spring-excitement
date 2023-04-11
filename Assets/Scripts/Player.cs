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
    [SerializeField] private float _punchForce;
    // ����� ������� ��������� � ������� �������� �����
    [SerializeField] private List<Enemy> _enemies;
    //��������
    [SerializeField] private float pause;
    private float time;



    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ������� ����� � �������� ����� �������
        if (Input.GetMouseButtonDown(0) && time <= 0)
        {
            Attack();
            time = pause;
        }
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
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
    }

    // ������� ����� ����� ��������� � ���� �������� �����
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject.CompareTag("Enemy"))
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
            if (_enemies[i] == null)
            {
                _enemies.Remove(_enemies[i]);
                continue;
            }
            // ��������� �����
            Vector3 vec = _enemies[i].transform.position - transform.position;
            float dis = vec.magnitude;
            Vector3 dir = vec / dis;
            _enemies[i].Damage(_punchDamage, dir * _punchForce);
        }
    }
}

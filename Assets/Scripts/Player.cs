using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float Speed;

    // боевая система
    [SerializeField] private float _punchDamage;
    [SerializeField] private float _punchForce;
    // враги которые находятся в радиусе действия удара
    [SerializeField] private List<Enemy> _enemies;
    //задержка
    [SerializeField] private float _pause;
    private float _time;
    //здоровье
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;

    public int Health
    {
        set
        {
            _health = value;
            if (_health > _maxHealth)
                _health = _maxHealth;
            if (_health < 0)
                Death();
        }
        get { return _health; }
    }


    private void Death()
    {
        
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _health = _maxHealth;
    }

    private void Update()
    {
        // простая атака и задержка между ударами
        if (Input.GetMouseButtonDown(0) && _time <= 0)
        {
            Attack();
            _time = _pause;
        }
        if (_time > 0f)
        {
            _time -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // движение
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * Speed);

        // поворот
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Quaternion deg = Quaternion.Euler(0, Mathf.Atan2(-y, x) * Mathf.Rad2Deg, 0);
        if (x != 0 || y != 0)
            _rigidbody.MoveRotation(deg);
    }

    // смотрим какие враги находятся в зоне действия атаки
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

    // простая атака
    void Attack()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == null)
            {
                _enemies.Remove(_enemies[i]);
                continue;
            }
            // получение урона
            Vector3 vec = _enemies[i].transform.position - transform.position;
            float dis = vec.magnitude;
            Vector3 dir = vec / dis;
            _enemies[i].Damage(_punchDamage, dir * _punchForce);
        }
    }
}

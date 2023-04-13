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

    // � ������� �� �����
    public bool InSnowDrift;

    // ������ �������
    [SerializeField] private float _punchDamage;
    //�������� ��� �����
    [SerializeField] private float _punchReload;
    private float _timePunchReloading;
    // �������� ����
    [SerializeField] private float _carpetPunchDamage;
    [SerializeField] private float _carpetPunchSpeed;
    [SerializeField] private int _carpetPunchLifeTime;
    [SerializeField] private int _carpetPunchReloadTime;
    private bool _carpetPunchReady;
    [SerializeField] private GameObject _carpetPunchPrefab;

    // ����� ������� ��������� � ������� �������� �����
    [SerializeField] private List<Enemy> _enemies;
    //��������
    [SerializeField] private int _maxHealth;
    private int _health;
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



    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _health = _maxHealth;
        StartCoroutine(CarpetRelaod());
    }

    private void Update()
    {
        // ������� ����� � �������� ����� �������
        if (Input.GetMouseButtonDown(0) && _timePunchReloading <= 0)
        {
            PunchAttack();
            _timePunchReloading = _punchReload;
        }
        if (_timePunchReloading > 0f)
        {
            _timePunchReloading -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _carpetPunchReady)
        {
            CarpetPunchAttack();
            _carpetPunchReady = false;
            StartCoroutine(CarpetRelaod());
        }
    }

    IEnumerator CarpetRelaod()
    {
        yield return new WaitForSeconds(_carpetPunchReloadTime);
        _carpetPunchReady = true;
    }

    private void CarpetPunchAttack()
    {
        GameObject carpet = Instantiate(_carpetPunchPrefab);
        carpet.transform.position = transform.position - new Vector3(0, transform.localScale.y / 1.5f, 0);
        Quaternion q = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
        carpet.transform.rotation = q;
        carpet.GetComponent<CarpetPunch>().Speed = _carpetPunchSpeed;
        carpet.GetComponent<CarpetPunch>().Damage = _carpetPunchDamage;
        carpet.GetComponent<CarpetPunch>().LifeTime = _carpetPunchLifeTime;
    }

    void FixedUpdate()
    {
        // ��������
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * Speed);

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
    void PunchAttack()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == null)
            {
                _enemies.Remove(_enemies[i]);
                continue;
            }
            // ��������� �����
            //Vector3 vec = _enemies[i].transform.position - transform.position;
            //float dis = vec.magnitude;
            //Vector3 dir = vec / dis;
            //_enemies[i].Damage(_punchDamage, dir * _punchForce);
        }
    }

    private void Death()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float Speed;
    // выбирает ли игрок квест
    [HideInInspector] public bool IsQuestChooseMenu;
    // дэш способность
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashReloadTime;
    private bool _dashReady;

    // в сугробе ли игрок
    public bool InSnowDrift;

    // боевая система
    [SerializeField] private float _punchDamage;
    //задержка при ударе
    [SerializeField] private float _punchReload;
    private float _timePunchReloading;
    // ковровый удар
    [SerializeField] private float _carpetPunchDamage;
    [SerializeField] private float _carpetPunchSpeed;
    [SerializeField] private int _carpetPunchLifeTime;
    [SerializeField] private int _carpetPunchReloadTime;
    private bool _carpetPunchReady;
    [SerializeField] private GameObject _carpetPunchPrefab;
    // супер молниеносный удра
    [SerializeField] private int _lightningDamage;
    [SerializeField] private int _lightningReloadTime;
    private bool _lightningReady;
    [SerializeField] private GameObject _cloudPrefab;

    // враги которые находятся в радиусе действия удара
    [SerializeField] private List<Enemy> _enemies;
    //здоровье
    public int _maxHealth;
    [SerializeField] int _health;
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
    // скорость восстановления здоровья
    [SerializeField] private int _healthRegenSpeed;

    // UI
    [SerializeField] private GameObject _pauseUI;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _health = _maxHealth;
        StartCoroutine(CarpetReload());
        StartCoroutine(LightningReload());
        StartCoroutine(HealthRegen());
        StartCoroutine(DashReload());
    }

    private void Update()
    {
        // простая атака и задержка между ударами
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
            StartCoroutine(CarpetReload());
        }

        if (Input.GetKeyDown(KeyCode.Q) && _lightningReady)
        {
            LightningAttack();
            _lightningReady = false;
            StartCoroutine(LightningReload());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashReady)
        {
            Dash();
            _dashReady = false;
            StartCoroutine(DashReload());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseUI.SetActive(!_pauseUI.activeSelf);
            Time.timeScale = _pauseUI.active ? 0 : 1;
        }
    }

    void FixedUpdate()
    {
        if(!IsQuestChooseMenu)
        {
            // движение
            Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * Speed);
        }

        // поворот
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Quaternion deg = Quaternion.Euler(0, Mathf.Atan2(x, y) * Mathf.Rad2Deg, 0);
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

    // дэш
    private void Dash()
    {
        _rigidbody.AddForce(transform.forward * _dashForce, ForceMode.Impulse);
    }

    IEnumerator DashReload()
    {
        yield return new WaitForSeconds(_dashReloadTime);
        _dashReady = true;
    }


    // АТАКИ

    IEnumerator CarpetReload()
    {
        yield return new WaitForSeconds(_carpetPunchReloadTime);
        _carpetPunchReady = true;
    }

    private void CarpetPunchAttack()
    {
        GameObject carpet = Instantiate(_carpetPunchPrefab);
        carpet.transform.position = transform.position - new Vector3(0, transform.localScale.y / 1.5f, 0);
        Quaternion q = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        carpet.transform.rotation = q;
        carpet.GetComponent<CarpetPunch>().Speed = _carpetPunchSpeed;
        carpet.GetComponent<CarpetPunch>().Damage = _carpetPunchDamage;
        carpet.GetComponent<CarpetPunch>().LifeTime = _carpetPunchLifeTime;
    }

    // простая атака
    void PunchAttack()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == null)
            {
                _enemies.Remove(_enemies[i]);
                continue;
            }
            else
                _enemies[i].Damage(_punchDamage);
            // нанесение урона врагу
            Vector3 vec = _enemies[i].transform.position - transform.position;
            float dis = vec.magnitude;
            Vector3 dir = vec / dis;
            _enemies[i].Damage(_punchDamage, dir * 10);
        }
    }

    IEnumerator LightningReload()
    {
        yield return new WaitForSeconds(_lightningReloadTime);
        _lightningReady = true;
    }

    private void LightningAttack()
    {
        GameObject cloud = Instantiate(_cloudPrefab);
        cloud.transform.position = transform.position +  new Vector3(0, 10, 0);
        cloud.GetComponent<Cloud>().Damage = _lightningDamage;
    }

    // реген хп
    IEnumerator HealthRegen()
    {
        while (true)
        {
            yield return new WaitForSeconds(_healthRegenSpeed);
            Health += 1;
        }
    }

    private void Death()
    {

    }



    public void ExitMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

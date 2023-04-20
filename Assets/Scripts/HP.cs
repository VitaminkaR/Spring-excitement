using TMPro.EditorUtilities;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class HP : MonoBehaviour
{
    private TrailRenderer _trainRenderer;
    private Light _light;

    [SerializeField] private Player _player;

    // если хп пролетит дальше этой дистанции, то оно начнет возвращаться к игроку
    [SerializeField] private float _returnDistance;
    // скорость хп, когда возвращается к игроку
    [SerializeField] private float _returnSpeed;
    // скорость хп
    [SerializeField] private float _speed;
    // длина линии
    [SerializeField] private float _startLength;
    // множитель сокращение длины линии
    [SerializeField] private float _speedLengthMultiplier;

    // цвета
    [SerializeField] private Color _healthyColor;
    [SerializeField] private Color _damagedColor;

    // догоняет или нет
    private bool _isReturning;

    private void Start()
    {
        _trainRenderer = GetComponent<TrailRenderer>();
        _light = GetComponent<Light>();
    }

    private void FixedUpdate()
    {
        // регулирование высоты
        transform.position = new Vector3(transform.position.x, _player.transform.position.y + 4, transform.position.z);
        
        // регулирования цвета и длины хп
        _trainRenderer.time = _startLength / _player._maxHealth * _player.Health;
        Color color = Color.Lerp(_damagedColor, _healthyColor, (float)_player.Health / (float)_player._maxHealth);
        _trainRenderer.material.color = color;
        _light.color = color;

        // математика
        Vector3 vec = new Vector3(_player.transform.position.x - transform.position.x, 0, _player.transform.position.z - transform.position.z);
        float distance = vec.magnitude;

        // контроль возврата
        if (distance > _returnDistance)
            _isReturning = true;
        if (distance < 0.5f)
            _isReturning = false;

        // движение
        if (_isReturning)
        {
            Vector3 dir = vec / distance;
            transform.Translate(dir * _returnSpeed * (distance / _returnDistance * 2) * Time.deltaTime);
            _trainRenderer.time = Mathf.Lerp(0, _trainRenderer.time, _speedLengthMultiplier);
        }
        else
        {
            Vector3 dir = new Vector3(-Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time));
            transform.Translate(dir * _speed* Time.deltaTime);
        }
    }
}

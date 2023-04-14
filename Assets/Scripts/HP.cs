using UnityEngine;

public class HP : MonoBehaviour
{
    private TrailRenderer _trainRenderer;
    private Light _light;

    [SerializeField] private Player _player;

    // ���� �� �������� ������ ���� ���������, �� ��� ������ ������������ � ������
    [SerializeField] private float _returnDistance;
    // �������� ��, ����� ������������ � ������
    [SerializeField] private float _returnSpeed;
    // �������� ��
    [SerializeField] private float _speed;
    // ��������� ������ ����� ������ �����������
    [SerializeField] private float _startLength;

    // �����
    [SerializeField] private Color _healthyColor;
    [SerializeField] private Color _damagedColor;

    // �������� ��� ���
    private bool _isReturning;

    private void Start()
    {
        _trainRenderer = GetComponent<TrailRenderer>();
        _light = GetComponent<Light>();
    }

    private void FixedUpdate()
    {
        _trainRenderer.time = _startLength / _player._maxHealth * _player.Health;
        Color color = Color.Lerp(_damagedColor, _healthyColor, (float)_player.Health / (float)_player._maxHealth);
        _trainRenderer.material.color = color;
        _light.color = color;

        Vector3 vec = new Vector3(_player.transform.position.x - transform.position.x, 0, _player.transform.position.z - transform.position.z);
        float distance = vec.magnitude;

        if (distance > _returnDistance)
            _isReturning = true;
        if (distance < 0.5f)
            _isReturning = false;

        if (_isReturning)
        {
            Vector3 dir = vec / distance;
            transform.Translate(dir * _returnSpeed * (distance / _returnDistance * 2) * Time.deltaTime);
            _trainRenderer.time -= _returnSpeed / 8;
        }
        else
        {
            Vector3 dir = new Vector3(-Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time));
            transform.Translate(dir * _speed* Time.deltaTime);
        }
    }
}
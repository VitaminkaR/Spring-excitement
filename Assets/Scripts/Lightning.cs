using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private LineRenderer _line;
    [SerializeField] private int _semgentsCount;
    public Vector3 _endPosition;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.SetVertexCount(_semgentsCount);

        Vector3 end = _endPosition;
        Vector3 step = end / _semgentsCount;
        Vector3 lastvec = new Vector3(0, 0, 0);
        _line.SetPosition(0, lastvec);
        for (int i = 1; i < _semgentsCount; i++)
        {
            _line.SetPosition(i, lastvec + step);
            lastvec += step;
        }
        int ofsx = 0;
        int ofsy = 0;
        if (Random.Range(-1f, 1f) < 0)
            ofsx = -1;
        else
            ofsx = 1;
        if (Random.Range(-1f, 1f) < 0)
            ofsy = -1;
        else
            ofsy = 1;
        float circlekof = Random.Range(-2f, 2f);

        for (int i = 1; i < _semgentsCount - 1; i++)
        {
            float x = Mathf.Sin(i * (Mathf.PI / (float)_semgentsCount)) * Random.Range(1f, 2f) * circlekof;
            _line.SetPosition(i, _line.GetPosition(i) + new Vector3 (x * ofsx, 0, x * ofsy) + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), Random.Range(-0.5f, 0.5f)));
        }
    }
}

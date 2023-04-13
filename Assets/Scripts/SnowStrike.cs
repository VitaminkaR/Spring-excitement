using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStrike : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _distanceDamage;

    [HideInInspector] public Player _player;

    private void Start()
    {
        StartCoroutine(Destruct());
        float dis = Vector3.Distance(transform.position, _player.transform.position);
        if(dis < _distanceDamage)
        {
            _player.Health -= _damage;
        }
    }

    IEnumerator Destruct()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

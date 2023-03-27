using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SerializeField] private int _health;
    [SerializeField] protected int _attackPower;

    void Update()
    {
        Death();
    }

    protected virtual void Attack()
    {

    }

    private void Death()
    {
        if (_health < 0)
        {
            Destroy(gameObject);
        }
    }
}

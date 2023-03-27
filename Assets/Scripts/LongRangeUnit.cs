using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongRangeUnit : Unit
{
    [SerializeField] private float _rechargeRate;
    [SerializeField] private GameObject _projectile;

    private void Start()
    {
        StartCoroutine(Reload());
    }
    protected override void Attack()
    {
        GameObject projectile = Instantiate(_projectile);
        projectile.transform.position = transform.position;
    }

    public IEnumerator Reload()
    {
        while (true)
        {
            yield return new WaitForSeconds(_rechargeRate);
            Attack();
        }
    }
}
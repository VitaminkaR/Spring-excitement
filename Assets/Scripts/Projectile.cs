using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _projectileVelocity;
    [SerializeField] private float _damage;

    private void Update()
    {
        transform.position -= new Vector3(0, 0, _projectileVelocity * Time.deltaTime);
        if (transform.position.z < -4)
        {
            Destroy(gameObject);
        }
    }
}

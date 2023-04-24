using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetPunch : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public int LifeTime;

    private void Start()
    {
        StartCoroutine(LifeCycle());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Damage(Damage);
        }
    }

    private void Update()
    {
        transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}

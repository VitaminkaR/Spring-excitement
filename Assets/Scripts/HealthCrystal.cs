using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrystal : MonoBehaviour
{
    // сколько восстанавливает здоровья
    [SerializeField] private int _health;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().Health += _health;
        StartCoroutine(Effect());
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<MeshRenderer>());
    }

    IEnumerator Effect()
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration + 1);
        Destroy(gameObject);
    }
}

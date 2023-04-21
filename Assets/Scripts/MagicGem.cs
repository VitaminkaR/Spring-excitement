using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Player>().Gems += 1;
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

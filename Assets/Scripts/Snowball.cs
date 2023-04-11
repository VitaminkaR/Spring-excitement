using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private int _damage;

    [SerializeField] private GameObject _snowStrike;
    [HideInInspector] public Player _player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Health -= _damage;
        }
        else
        {
            GameObject snowStrike = Instantiate(_snowStrike);
            snowStrike.transform.position = transform.position;
            snowStrike.GetComponent<SnowStrike>()._player = _player;
        }
        Destroy(gameObject);
    }
}

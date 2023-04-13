using Unity.VisualScripting;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private int _damage;

    [SerializeField] private GameObject _snowStrike;
    [HideInInspector] public Player _player;

    private void OnTriggerEnter(Collider collider)
    {
        // должен сталкивать только с окружение и игроком
        if (collider.gameObject.CompareTag("Enemy"))
            return;

        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Player>().Health -= _damage;
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

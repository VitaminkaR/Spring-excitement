using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SnowballAttack : MonoBehaviour
{
    [SerializeField]  private Enemy _enemy;
    [SerializeField] private int _attackFrequence;
    [SerializeField] private float _snowballSpeed;
    [SerializeField] private GameObject _snowballObj;

    void Start()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (_enemy._seePlayer)
            {
                GameObject snowball = Instantiate(_snowballObj);
                Vector3 postition = transform.position + new Vector3(0, 2, 0);
                Rigidbody snowballRigidbody = snowball.GetComponent<Rigidbody>();
                snowball.transform.position = postition;
                snowball.GetComponent<Snowball>()._player = _enemy._player;

                Vector3 vec = _enemy._player.transform.position - snowball.transform.position;
                float dis = vec.magnitude;
                Vector3 dir = vec / dis;
                snowballRigidbody.AddForce(Vector3.up * dis / (_snowballSpeed / 6f), ForceMode.Impulse);
                snowballRigidbody.AddForce(dir * _snowballSpeed, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(_attackFrequence);
        }
    }
}

using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private ParticleSystem _cloud;
    private ParticleSystem _rain;
    [SerializeField] private GameObject _lightning;

    [HideInInspector] public int Damage;

    private void Start()
    {
        _cloud = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        _rain = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

        StartCoroutine(Plot());
    }

    IEnumerator Plot()
    {
        _cloud.Play();
        yield return new WaitForSeconds(1.5f);
        _rain.Play();
        yield return new WaitForSeconds(1.5f);

        _cloud.Pause();

        LayerMask mask = LayerMask.GetMask("Ignore Raycast");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 50, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null && !colliders[i].isTrigger && colliders[i].gameObject.CompareTag("Enemy"))
            {
                GameObject lightning = Instantiate(_lightning);
                lightning.transform.position = transform.position;
                lightning.GetComponent<Lightning>()._endPosition = colliders[i].gameObject.transform.position - transform.position;
                colliders[i].gameObject.GetComponent<Enemy>().Damage(Damage);
                yield return new WaitForSeconds(0.25f);
                Destroy(lightning);
            }
        }

        _cloud.Play();
        _rain.Stop();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

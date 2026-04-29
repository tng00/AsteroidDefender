using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform target;
    public float spawnRadius = 20f;
    public float spawnInterval = 2f;
    public float minInterval = 0.6f;

    private float _timer;
    private float _currentInterval;

    void Start()
    {
        _currentInterval = spawnInterval;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _currentInterval)
        {
            _timer = 0f;
            Spawn();

            _currentInterval = Mathf.Max(minInterval, _currentInterval - 0.02f);
        }
    }

    void Spawn()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float height = Random.Range(-10f, 50f);
        Vector3 spawnPos = new Vector3(
            transform.position.x + Mathf.Cos(angle) * spawnRadius,
            transform.position.y + height,
            transform.position.z + Mathf.Sin(angle) * spawnRadius
        );

        GameObject ast = Instantiate(asteroidPrefab, spawnPos, Random.rotation);

        if (target != null)
        {
            Vector3 dir = (target.position - spawnPos).normalized;
            dir += new Vector3(
                Random.Range(-0.1f, 0.1f),
                0f,
                Random.Range(-0.1f, 0.1f)
            );
            float speed = Random.Range(3f, 7f);
            ast.GetComponent<Rigidbody>().linearVelocity = dir * speed;
        }
    }
}
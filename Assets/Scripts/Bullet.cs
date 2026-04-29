using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public int damage = 1;
    public float lifetime = 4f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
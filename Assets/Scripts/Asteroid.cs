using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int health = 1;
    public int scoreValue = 10;
    public GameObject explosionVFX;  
    public AudioClip explosionSound;

    private bool _isDead = false; 

    void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * Random.Range(1f, 4f);
    }

    public void TakeDamage(int dmg)
    {
        if (_isDead) return;

        health -= dmg;
        if (health <= 0) 
        {
            _isDead = true;
            Die(true);
        }
    }

    void Die(bool addScore)
    {
        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        
        if (addScore) 
        {
            GameManager.Instance?.AddScore(scoreValue);
        }
        
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isDead) return;

        if (other.CompareTag("Station"))
        {
            _isDead = true; 
            GameManager.Instance?.DamageStation(1);
            Die(false);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerShooter : MonoBehaviour
{
    [Header("Настройки стрельбы")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.0f;
    public AudioClip shootSound;

    [Header("Настройки прицеливания")]
    public LayerMask aimLayer;
    public float rotationSpeed = 10f;
    public float maxAimDistance = 1000f; 

    private float _nextFire;
    private AudioSource _audio;
    private Camera _cam;
    private Vector3 _aimPoint;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _cam = Camera.main;
    }

    void Update()
    {
        AimAtMouse();
        HandleShooting();
    }

    void AimAtMouse()
    {
        if (_cam == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxAimDistance, aimLayer, QueryTriggerInteraction.Collide))
        {
            _aimPoint = hit.point;
            Debug.Log("<color=green>МЫШКА НАВЕДЕНА НА АСТЕРОИД: </color>" + hit.collider.gameObject.name);
        }
        else
        {
            Plane aimPlane = new Plane(Vector3.up, firePoint.position);
            
            if (aimPlane.Raycast(ray, out float dist))
            {
                _aimPoint = ray.GetPoint(dist);
            }
            else
            {
                _aimPoint = ray.origin + ray.direction * maxAimDistance;
            }
        }

        Vector3 dir = (_aimPoint - transform.position);
        dir.y = 0;
        
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Debug.DrawLine(firePoint.position, _aimPoint, Color.yellow);
    }

    void HandleShooting()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }

        if (Mouse.current.leftButton.isPressed && Time.time >= _nextFire)
        {
            _nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 directionToAim = (_aimPoint - firePoint.position).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(directionToAim);

        Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        if (_audio && shootSound) _audio.PlayOneShot(shootSound);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(_aimPoint, 0.5f);
    }
}
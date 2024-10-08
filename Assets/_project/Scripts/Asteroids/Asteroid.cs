using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    [SerializeField] private FracturedAsteroid _fracturedAsteroidPrefab;
    [SerializeField] private Detonator _explosionPrefab;

    private Transform _transform;
    Rigidbody rb;
    //void Start()
    //{
    //    Rigidbody rb = GetComponent<Rigidbody>();
       
    //}
    private void Awake()
    {
        _transform = transform;
    }

    public void TakeDamage(int damage, Vector3 hitPosition)
    {
        FractureAsteroid(hitPosition);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (rb != null)
    //    {
    //        rb.isKinematic = true;
    //    }
    //}

    private void FractureAsteroid(Vector3 hitPosition)
    {
        if (_fracturedAsteroidPrefab != null)
        {
            Instantiate(_fracturedAsteroidPrefab, _transform.position, _transform.rotation);
        }

        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab, transform.position/*hitPosition*/, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
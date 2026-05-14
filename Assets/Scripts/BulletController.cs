using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;
    
    private int damage;
    private float spawnTime;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= spawnTime + lifeTime)
        {
            ReturnToPool();
        }
    }

    public void Launch(Vector3 direction, int damageAmount)
    {
        damage = damageAmount;
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;
        
        if (other.CompareTag("Bullet")) return;

        if (other.CompareTag("Enemy"))
        {
            Damage_Interface damagable = other.GetComponent<Damage_Interface>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
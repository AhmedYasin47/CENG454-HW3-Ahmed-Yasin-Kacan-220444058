using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f; 
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float resumeDistance = 0.5f;
    
    [Header("Saldırı Ayarları")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackCooldown = 1.5f; // Animation Event kullanmazsan devreye girer
    [SerializeField] private bool useAnimationEvent = true; // Animation event mi yoksa cooldown mu?

    private Transform coreTarget;
    private MovementStrategy_Interface currentStrategy;
    private Animator animator;
    
    private Rigidbody rb; 
    private Collider coreCollider;
    private Collider myCollider;
    private CoreHealth coreHealth;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    void Awake()
    {
        CoreHealth core = FindFirstObjectByType<CoreHealth>();
        if (core != null)
        {
            coreTarget = core.transform;
            coreCollider = core.GetComponent<Collider>();
            coreHealth = core;
        }

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>(); 
        myCollider = GetComponent<Collider>();
    }

    void Start()
    {
        SetStrategy(new ZigZagMovementStrategy());
    }

    void Update()
    {
        if (coreTarget == null || currentStrategy == null) return;

        float distance = GetDistanceToCore();

        // Hysteresis: titreşimi önler
        if (isAttacking)
        {
            if (distance > resumeDistance)
            {
                isAttacking = false;
            }
        }
        else
        {
            if (distance <= stoppingDistance)
            {
                isAttacking = true;
                lastAttackTime = Time.time; // İlk vuruşta hemen vurmasın diye reset
            }
        }

        if (!isAttacking)
        {
            // HAREKET MODU
            currentStrategy.Move(this.transform, coreTarget, speed);
            if (animator != null) animator.SetBool("isAttacking", false);

            Vector3 lookDirection = coreTarget.position - transform.position;
            lookDirection.y = 0; 
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        else
        {
            // SALDIRI MODU
            if (animator != null) animator.SetBool("isAttacking", true);
            
            Vector3 lookDirection = coreTarget.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
            
            if (rb != null) 
            {
                rb.linearVelocity = Vector3.zero; 
                rb.angularVelocity = Vector3.zero;
            }

            // Animation Event kullanmıyorsak, cooldown ile vur
            if (!useAnimationEvent)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    DealDamage();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    // Animation Event'in çağıracağı metod (vuruşun tam temas frame'ine bağlanır)
    public void DealDamage()
    {
        // Sadece saldırı modundayken ve core hala varsa hasar ver
        if (!isAttacking || coreHealth == null) return;
        
        coreHealth.TakeDamage(damageAmount);
    }

    private float GetDistanceToCore()
    {
        if (coreCollider == null || myCollider == null)
        {
            return Vector3.Distance(transform.position, coreTarget.position);
        }

        Vector3 closestPointOnCore = coreCollider.ClosestPoint(transform.position);
        Vector3 closestPointOnMe = myCollider.ClosestPoint(closestPointOnCore);
        return Vector3.Distance(closestPointOnMe, closestPointOnCore);
    }

    public void SetStrategy(MovementStrategy_Interface newStrategy)
    {
        currentStrategy = newStrategy;
    }

    void OnEnable()
    {
        SetStrategy(new ZigZagMovementStrategy());
        isAttacking = false;
        if (animator != null) animator.SetBool("isAttacking", false);

        if (coreTarget != null)
        {
            Vector3 lookDirection = coreTarget.position - transform.position;
            lookDirection.y = 0; 
            
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
}
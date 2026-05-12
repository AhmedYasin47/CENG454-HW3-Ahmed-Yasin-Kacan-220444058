using UnityEngine;  
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 3f; 
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float resumeDistance = 0.5f;
    
    [Header("Saldırı Ayarları")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private bool useAnimationEvent = true;

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
            lastAttackTime = Time.time - attackCooldown;
    }
        }

        if (!isAttacking)
        {
            currentStrategy.Move(this.transform, coreTarget, speed);
            SetAnimatorBool("isAttacking", false);

            Vector3 lookDirection = coreTarget.position - transform.position;
            lookDirection.y = 0; 
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        else
        {
            SetAnimatorBool("isAttacking", true);
            
            Vector3 lookDirection = coreTarget.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
            
            if (rb != null && !rb.isKinematic) 
            {
                rb.linearVelocity = Vector3.zero; 
                rb.angularVelocity = Vector3.zero;
            }

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

    public void DealDamage()
{
    Debug.Log("DealDamage çağrıldı! isAttacking: " + isAttacking + ", coreHealth: " + (coreHealth != null));
    
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
    private void SetAnimatorBool(string paramName, bool value)
    {
        if (animator == null) return;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(paramName, value);
                return;
            }
        }
    }

    public void SetStrategy(MovementStrategy_Interface newStrategy)
    {
        currentStrategy = newStrategy;
    }

    void OnEnable()
    {
        SetStrategy(new ZigZagMovementStrategy());
        isAttacking = false;
        SetAnimatorBool("isAttacking", false);

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
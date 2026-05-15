using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private LayerMask aimLayerMask;

    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;
    
    private Weapon_Interface myWeapon;
    private float lastFireTime;
    private Camera mainCamera;
    private Animator animator; 

    void Start()
    {
        myWeapon = new BasicWeapon();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleShooting();
        HandleUpgrade();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveDirection.z = 1f;
        if (Keyboard.current.sKey.isPressed) moveDirection.z = -1f;
        if (Keyboard.current.aKey.isPressed) moveDirection.x = -1f;
        if (Keyboard.current.dKey.isPressed) moveDirection.x = 1f;
        
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        bool isMoving = moveDirection != Vector3.zero;
        SetAnimatorBool("isRunning", isMoving);
    }

    private void HandleAiming()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 lookDirection = hitPoint - transform.position;
            lookDirection.y = 0;
            
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    private void HandleShooting()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            if (Time.time >= lastFireTime + fireRate)
            {
                Shoot();
                lastFireTime = Time.time;
            }
        }
    }

    private void Shoot()
{
    if (firePoint == null)
    {
        Debug.LogWarning("FirePoint atanmamış!");
        return;
    }

    int damage = myWeapon.GetDamage();
    
    GameObject bullet = BulletPool.Instance.GetBullet();
    bullet.transform.position = firePoint.position;
    bullet.transform.rotation = firePoint.rotation;
    bullet.SetActive(true);
    
    BulletController bulletCtrl = bullet.GetComponent<BulletController>();
    if (bulletCtrl != null)
    {
        bulletCtrl.Launch(firePoint.forward, damage);
    }
    
    if (SoundManager.Instance != null)
    {
        SoundManager.Instance.PlayShoot();
    }
}

    private void HandleUpgrade()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            myWeapon = new DamageBoostDecorator(myWeapon);
            Debug.Log("GÜÇLENDİRME ALINDI! Yeni hasar: " + myWeapon.GetDamage());
        }
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
}
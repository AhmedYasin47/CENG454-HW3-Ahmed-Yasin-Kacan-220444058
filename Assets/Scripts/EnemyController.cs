using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    private Transform coreTarget;
    
    private MovementStrategy_Interface currentStrategy;

    void Start()
    {

        CoreHealth core = FindFirstObjectByType<CoreHealth>();
        if (core != null)
        {
            coreTarget = core.transform;
        }

        SetStrategy(new ZigZagMovementStrategy());
    }

    void Update()
    {
        if (coreTarget != null && currentStrategy != null)
        {
            currentStrategy.Move(this.transform, coreTarget, speed);
        }
    }

    public void SetStrategy(MovementStrategy_Interface newStrategy)
    {
        currentStrategy = newStrategy;
    }
    void OnEnable()
    {
        SetStrategy(new ZigZagMovementStrategy());
    }
}
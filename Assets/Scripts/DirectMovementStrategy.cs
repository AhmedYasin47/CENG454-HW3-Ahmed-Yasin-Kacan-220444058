using UnityEngine;

public class DirectMovementStrategy : MovementStrategy_Interface
{
    public void Move(Transform enemy, Transform target, float speed)
    {
        Vector3 targetPosition = target.position;
        targetPosition.y = enemy.position.y;
        
        enemy.position = Vector3.MoveTowards(enemy.position, targetPosition, speed * Time.deltaTime);
    }
}
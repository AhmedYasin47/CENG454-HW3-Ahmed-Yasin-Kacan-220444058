using UnityEngine;

public class DirectMovementStrategy : MovementStrategy_Interface
{
    public void Move(Transform enemy, Transform target, float speed)
    {
        enemy.position = Vector3.MoveTowards(enemy.position, target.position, speed * Time.deltaTime);
    }
}
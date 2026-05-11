using UnityEngine;

public class ZigZagMovementStrategy : MovementStrategy_Interface
{
    private float zigzagWidth = 1.5f;
    private float zigzagFrequency = 3f;

    public void Move(Transform enemy, Transform target, float speed)
    {
        Vector3 directionToTarget = (target.position - enemy.position).normalized;
        directionToTarget.y = 0;
        
        Vector3 forwardMove = directionToTarget * speed * Time.deltaTime;

        Vector3 rightDirection = Vector3.Cross(Vector3.up, directionToTarget).normalized;

        float wave = Mathf.Sin(Time.time * zigzagFrequency) * zigzagWidth;
        Vector3 sideMove = rightDirection * wave * Time.deltaTime;

        enemy.position += forwardMove + sideMove;
    }
}
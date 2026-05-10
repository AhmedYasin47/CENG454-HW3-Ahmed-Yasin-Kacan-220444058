using UnityEngine;

public class ZigZagMovementStrategy : MovementStrategy_Interface
{
    private float zigzagWidth = 10f;
    private float zigzagFrequency = 5f;

    public void Move(Transform enemy, Transform target, float speed)
    {
        Vector3 directionToTarget = (target.position - enemy.position).normalized;
        
        Vector3 forwardMove = directionToTarget * speed * Time.deltaTime;

        Vector3 rightDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized;

        float wave = Mathf.Sin(Time.time * zigzagFrequency) * zigzagWidth * Time.deltaTime;
        Vector3 sideMove = rightDirection * wave;

        enemy.position += forwardMove + sideMove;
    }
}
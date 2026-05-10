using UnityEngine;

public interface MovementStrategy_Interface
{
    void Move(Transform enemy, Transform target, float speed);
}

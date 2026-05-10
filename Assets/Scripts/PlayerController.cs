using UnityEngine;
using UnityEngine.InputSystem; // Yeni girdi sistemini kullanmak için zorunlu

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) moveDirection.z = 1f;
        if (Keyboard.current.sKey.isPressed) moveDirection.z = -1f;
        if (Keyboard.current.aKey.isPressed) moveDirection.x = -1f;
        if (Keyboard.current.dKey.isPressed) moveDirection.x = 1f;

        moveDirection = moveDirection.normalized;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        // TEST İÇİN: Boşluk tuşuna basıldığında Çekirdeğe hasar ver.
    if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Sahnedeki CoreHealth scriptini bul
            CoreHealth core = FindFirstObjectByType<CoreHealth>();
        if (core != null)
            {
                core.TakeDamage(10); // 10 hasar vur
            }
        }
    }
}
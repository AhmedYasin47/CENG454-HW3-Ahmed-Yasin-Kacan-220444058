using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Weapon_Interface myWeapon;

    void Start()
    {
        myWeapon = new BasicWeapon();
    }

    void Update()
    {

        Vector3 moveDirection = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) moveDirection.z = 1f;
        if (Keyboard.current.sKey.isPressed) moveDirection.z = -1f;
        if (Keyboard.current.aKey.isPressed) moveDirection.x = -1f;
        if (Keyboard.current.dKey.isPressed) moveDirection.x = 1f;
        
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            int currentDamage = myWeapon.GetDamage();
            Debug.Log("Silah Ateşlendi! Verilen Hasar: " + currentDamage);
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            myWeapon = new DamageBoostDecorator(myWeapon);
            Debug.Log("GÜÇLENDİRME ALINDI! Artık silah daha güçlü.");
        }
    }
}
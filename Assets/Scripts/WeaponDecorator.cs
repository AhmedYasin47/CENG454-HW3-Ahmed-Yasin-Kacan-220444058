public abstract class WeaponDecorator : Weapon_Interface
{
    protected Weapon_Interface wrappedWeapon;

    public WeaponDecorator(Weapon_Interface weapon)
    {
        this.wrappedWeapon = weapon;
    }

    public virtual int GetDamage()
    {
        return wrappedWeapon.GetDamage();
    }
}
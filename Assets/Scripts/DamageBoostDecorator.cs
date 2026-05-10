public class DamageBoostDecorator : WeaponDecorator
{
    public DamageBoostDecorator(Weapon_Interface weapon) : base(weapon)
    {
        
    }

    public override int GetDamage()
    {
        return base.GetDamage() + 15; 
    }
}
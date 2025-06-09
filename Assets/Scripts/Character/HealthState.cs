
public class HealthState : IBehavior
{
    private float health;
    private float defense;

    public float Defense { get => defense; set => defense = value; }
    public float Health { get => health; set => health = value; }

    public void SetBehavior()
    {
        
    }
    
    public void TakeDamage(float damage)
    {
        if (health <= 0)
        {
            //isAlive = fasle;
            return;
        }
        health -= (damage-defense);
    }
    public void Heal(float healthToHeal, bool canHeal = false)
    {
        if (canHeal) //&& isAlive
        {
            health += healthToHeal;
        }
    }
}

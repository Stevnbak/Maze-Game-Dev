using UnityEngine;
public interface ICreature
{
    float health { get; set; }
    float maxHealth { get; set; }
    void TakeDamage(float damage);
}

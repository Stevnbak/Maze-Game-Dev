using UnityEngine;
public interface ICreature
{
    float health { get; set; }
    void TakeDamage(float damage);
}

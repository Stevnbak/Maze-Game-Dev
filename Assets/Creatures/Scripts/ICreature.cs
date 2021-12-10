public interface ICreature
{
    void Initialize();
    float health { get; set; }
    void TakeDamage(float damage);
}

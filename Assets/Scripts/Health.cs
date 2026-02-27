using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    public int CurrentHP { get; private set; }

    public UnityEvent<int, int> onHealthChanged; // current, max
    public UnityEvent onDied;

    [Header("Death")]
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelay = 0f;

    [Header("Debug")]
    [SerializeField] private bool logDamage = true;

    private bool dead;

    private void Awake()
    {
        CurrentHP = maxHP;
        onHealthChanged?.Invoke(CurrentHP, maxHP);
    }

    public void TakeDamage(int dmg)
    {
        if (dead) return;

        dmg = Mathf.Max(0, dmg);
        if (dmg == 0) return;

        int before = CurrentHP;
        CurrentHP -= dmg;
        if (CurrentHP < 0) CurrentHP = 0;

        onHealthChanged?.Invoke(CurrentHP, maxHP);

        if (logDamage)
            Debug.Log($"{name} took {dmg} damage. HP: {before} -> {CurrentHP}/{maxHP}");

        if (CurrentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (dead) return;

        amount = Mathf.Max(0, amount);
        if (amount == 0) return;

        int before = CurrentHP;
        CurrentHP = Mathf.Min(maxHP, CurrentHP + amount);

        onHealthChanged?.Invoke(CurrentHP, maxHP);

        if (logDamage)
            Debug.Log($"{name} healed {amount}. HP: {before} -> {CurrentHP}/{maxHP}");
    }

    private void Die()
    {
        if (dead) return;
        dead = true;

        if (logDamage)
            Debug.Log($"{name} died.");

        onDied?.Invoke();

        if (destroyOnDeath)
            Destroy(gameObject, destroyDelay);
    }
}
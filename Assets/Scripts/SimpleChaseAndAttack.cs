using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleChaseAndAttack : MonoBehaviour
{
    [Header("Targeting")]
    public Team targetTeam = Team.Enemy;
    public float detectRadius = 12f;
    public float attackRange = 2.8f; // 3D'de daha stabil

    [Header("Combat")]
    public float attackCooldown = 1.0f;
    public int damage = 10;

    private NavMeshAgent agent;
    private Health myHealth;
    private Unit selfUnit;

    private float nextAttackTime;
    private Transform currentTarget;
    private Health targetHealth;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        myHealth = GetComponent<Health>();
        selfUnit = GetComponent<Unit>();

        // Agent ayarlarý
        agent.updateRotation = true;
        agent.updateUpAxis = true;
        agent.stoppingDistance = attackRange * 0.9f;
    }

    private void Update()
    {
        if (myHealth != null && myHealth.CurrentHP <= 0) return;

        if (currentTarget == null)
            AcquireTarget();

        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.position);

        if (dist > attackRange)
        {
            if (agent.isStopped) agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
        }
        else
        {
            if (!agent.isStopped) agent.isStopped = true;
            TryAttack();
        }
    }

    private void AcquireTarget()
    {
        // Performanslý yöntem: UnitRegistry'den en yakýn hedefi al
        if (selfUnit == null) return;

        if (UnitRegistry.TryGetNearest(selfUnit, targetTeam, detectRadius, out var nearest))
        {
            currentTarget = nearest.transform;
            targetHealth = nearest.GetComponent<Health>();
        }
        else
        {
            currentTarget = null;
            targetHealth = null;
        }
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        if (targetHealth != null)
            targetHealth.TakeDamage(damage);

        // hedef öldüyse hedefi düþür
        if (targetHealth != null && targetHealth.CurrentHP <= 0)
        {
            currentTarget = null;
            targetHealth = null;
        }
    }
}
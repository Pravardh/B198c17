using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 4f;

    private PlayerScoreSystem playerScoreSystem;

    private EnemyHealthComponent healthComponent;

    private Transform playerTransform;
    private NavMeshAgent navMesh;
    private NPCAnimation npcAnimation;

    private bool isDead = false;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        npcAnimation = GetComponent<NPCAnimation>();

        healthComponent = GetComponent<EnemyHealthComponent>();
        healthComponent.OnDeath += Die;
        playerTransform = GameObject.Find("Player")?.transform;

        playerScoreSystem = playerTransform.GetComponent<PlayerScoreSystem>();


    }

    private void Update()
    {
        if (isDead || playerTransform == null)
            return;

        float playerDistance = Vector3.Distance(
            transform.position,
            playerTransform.position
        );


        if (playerDistance <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        navMesh.isStopped = false;
        navMesh.SetDestination(playerTransform.position);

        LookAt(playerTransform);

        npcAnimation.SetWalking(false);
        npcAnimation.SetRunning(true);
        npcAnimation.SetChasing(true);
        npcAnimation.SetAttacking(false);

    }

    private void AttackPlayer()
    {
        navMesh.isStopped = true;

        LookAt(playerTransform);

        npcAnimation.SetWalking(false);
        npcAnimation.SetRunning(false);
        npcAnimation.SetChasing(false);
        npcAnimation.SetAttacking(true);

    }

    public void DamagePlayer()
    {
        playerTransform.GetComponent<PlayerHealthComponent>().TakeDamage();
    }

    private void LookAt(Transform target)
    {
        Vector3 direction =
            target.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            5f * Time.deltaTime
        );
    }

    public void Die()
    {
        if (isDead)
            return;

        
        isDead = true;
        playerScoreSystem?.AddScore(1);

        navMesh.isStopped = true;
        navMesh.ResetPath();
    }
}
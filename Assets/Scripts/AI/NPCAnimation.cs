using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator animator;

    private static readonly int IsWalkingHash =
        Animator.StringToHash("IsWalking");

    private static readonly int IsRunningHash =
        Animator.StringToHash("IsRunning");

    private static readonly int IsAttackingHash =
        Animator.StringToHash("IsAttacking");

    private static readonly int IsChasingHash =
        Animator.StringToHash("IsChasing");

    private static readonly int TakeDamageHash =
        Animator.StringToHash("TakeDamage");

    private static readonly int DeathHash =
        Animator.StringToHash("Death");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalking(bool value)
    {
        animator.SetBool(IsWalkingHash, value);
    }

    public void SetRunning(bool value)
    {
        animator.SetBool(IsRunningHash, value);
    }

    public void SetAttacking(bool value)
    {
        animator.SetBool(IsAttackingHash, value);
    }

    public void SetChasing(bool value)
    {
        animator.SetBool(IsChasingHash, value);
    }

    public void TakeDamage()
    {
        animator.SetTrigger(TakeDamageHash);
    }

    public void Die()
    {
        animator.SetBool(IsWalkingHash, false);
        animator.SetBool(IsRunningHash, false);
        animator.SetBool(IsAttackingHash, false);
        animator.SetBool(IsChasingHash, false);

        animator.SetTrigger(DeathHash);
    }
}
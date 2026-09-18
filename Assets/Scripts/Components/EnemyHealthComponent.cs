using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField]
    private NPCAnimation npcAnimation;

    public event Action OnDeath;
    private void Awake()
    {
        if (npcAnimation == null)
            npcAnimation = GetComponent<NPCAnimation>();
    }

    public void TakeDamage()
    {
        Debug.Log("Taking damage from enemy");
        Die();
    }

    protected void Die()
    {
        if (npcAnimation != null)
        {
            OnDeath?.Invoke();
            npcAnimation.Die();
            Debug.Log("Dying");
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 3.0f);
        }
    }
}
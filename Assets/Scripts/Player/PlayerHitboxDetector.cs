using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxDetector : MonoBehaviour
{
    [SerializeField]
    private List<EnemyHealthComponent> targetHealthComponents = new();

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger component: " + other.gameObject);
        if(other.TryGetComponent(out EnemyHealthComponent healthComponent))
        {
            if(!targetHealthComponents.Contains(healthComponent))
            {
                Debug.Log("Adding health component");
                targetHealthComponents.Add(healthComponent);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out EnemyHealthComponent healthComponent))
        {
            if(targetHealthComponents.Contains(healthComponent))
            {
                targetHealthComponents.Remove(healthComponent);
            }

        }
    }

    public List<EnemyHealthComponent> GetTargetHealthComponents()
    {
        return targetHealthComponents;
    }
}

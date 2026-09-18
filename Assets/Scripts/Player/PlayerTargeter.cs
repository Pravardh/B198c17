using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class PlayerTargeter : MonoBehaviour
{
    [SerializeField]
    private ThirdPersonCameraController playerCameraController;

    [SerializeField]
    private PlayerInputReader playerInputReader;
    [SerializeField]
    private List<Collider> overlappedColliders = new();
    private Transform target;


    void Start()
    {
        playerInputReader.OnPlayerTarget += ToggleTarget;
    }

    private void ToggleTarget()
    {
        if(target == null)
        {
            StartTarget();
        }
        else
        {
            StopTarget();
        }
    }

    public void StartTarget()
    {
        float distance = 10000;
        Transform closestTransform = null;

        foreach(Collider collider in overlappedColliders)
        {
            if(Mathf.Abs(Vector3.Distance(collider.transform.position, transform.position)) < distance)
            {
                closestTransform = collider.transform;
                distance = Mathf.Abs(Vector3.Distance(collider.transform.position, transform.position));
            }
        }

        if(closestTransform != null)
        {
            target = closestTransform;
            playerCameraController.SetLookAtTarget(closestTransform);
        }
    }

    public void StopTarget()
    {
        playerCameraController.ClearTarget();
        target = null;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(!overlappedColliders.Contains(other))
        {
            overlappedColliders.Add(other);
        }    
    }

    void OnTriggerExit(Collider other)
    {
        if(overlappedColliders.Contains(other))
        {
            overlappedColliders.Remove(other);

            if(target == other.transform)
            {
                StopTarget();
                target = null;            
            }
        }
    }

}

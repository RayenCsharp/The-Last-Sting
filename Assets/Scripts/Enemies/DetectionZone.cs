using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private LayerMask detectionLayer;

    public List<Collider> detectedColliders = new List<Collider>();

    private Collider col;


    void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & detectionLayer) != 0)
        {
            if (!detectedColliders.Contains(other))
            {
                detectedColliders.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (detectedColliders.Contains(other))
        {
            detectedColliders.Remove(other);
        }
    }
}

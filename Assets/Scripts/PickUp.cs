using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float verticalAmplitude = 0.2f;
    [SerializeField] private float verticalFrequency = 4f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        transform.position = startPosition +
            Vector3.up * Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
    }
}
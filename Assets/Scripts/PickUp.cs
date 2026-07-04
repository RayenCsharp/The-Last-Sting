using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float verticalAmplitude = 0.2f;
    [SerializeField] private float verticalFrequency = 4f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude + 1f, transform.position.z);
    }
}

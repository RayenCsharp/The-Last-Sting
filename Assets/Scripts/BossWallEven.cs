using UnityEngine;

public class BossWallEven : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private DetectionZone detectionZone;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform Sun;

    [SerializeField] private float targetSunRotationX = 180f;
    [SerializeField] private float sunRotationSpeed = 10f;
    private bool hasTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasTriggered && detectionZone.detectedColliders.Count > 0)
        {
            hasTriggered = true;
            wall.SetActive(true);


            audioSource.clip = bossMusic;
            audioSource.Play();

        }

        if (hasTriggered)
        {
            NightAtmosphere();
        }
    }


    void NightAtmosphere()
    {
        Quaternion targetRotation = Quaternion.Euler(targetSunRotationX, Sun.rotation.eulerAngles.y, Sun.rotation.eulerAngles.z);
        Sun.rotation = Quaternion.RotateTowards(Sun.rotation, targetRotation, sunRotationSpeed * Time.deltaTime);
    }


}

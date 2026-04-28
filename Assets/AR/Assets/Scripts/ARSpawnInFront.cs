using UnityEngine;

public class ARSpawnInFront : MonoBehaviour
{
    public static ARSpawnInFront Instance;

    public Camera arCamera;
    public float spawnDistance = 1.5f;

    private GameObject spawnedObject;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnPlanet();
    }

    public void SpawnPlanet()
    {
        if (PlanetDataHolder.Instance.selectedPlanetPrefab == null)
            return;

        // Destroy previous
        if (spawnedObject != null)
            Destroy(spawnedObject);

        Vector3 spawnPos = arCamera.transform.position +
                           arCamera.transform.forward * spawnDistance;

        Quaternion rotation = Quaternion.LookRotation(arCamera.transform.forward);

        spawnedObject = Instantiate(
            PlanetDataHolder.Instance.selectedPlanetPrefab,
            spawnPos,
            rotation
        );
        spawnedObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        // Set selected
        ARObjectManipulator.selectedObject = spawnedObject;
    }

    public void ResetToCenter()
    {
        if (spawnedObject == null || arCamera == null)
            return;

        Vector3 newPos = arCamera.transform.position +
                         arCamera.transform.forward * spawnDistance;

        Quaternion newRot = Quaternion.LookRotation(arCamera.transform.forward);

        spawnedObject.transform.position = newPos;
        spawnedObject.transform.rotation = newRot;
    }
}
using UnityEngine;

public class PlanetDataHolder : MonoBehaviour
{
    public static PlanetDataHolder Instance;

    public GameObject selectedPlanetPrefab;
    public string planetName;
    public string planetInfo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlanet(GameObject prefab, string name, string info)
    {
        selectedPlanetPrefab = prefab;
        planetName = name;
        planetInfo = info;
    }
}
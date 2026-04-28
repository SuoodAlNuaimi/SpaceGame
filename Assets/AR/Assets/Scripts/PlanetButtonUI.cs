using UnityEngine;

public class PlanetButtonUI : MonoBehaviour
{
    public GameObject planetPrefab;
    [TextArea] public string planetInfo;
    public string planetName;

    public void OnClickPlanet()
    {
        // Update global data
        PlanetDataHolder.Instance.SetPlanet(
            planetPrefab,
            planetName,
            planetInfo
        );

        // Spawn new planet
        ARSpawnInFront.Instance.SpawnPlanet();

        // Update UI text
        FindObjectOfType<PlanetUI>().RefreshUI();
    }
}
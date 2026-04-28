using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelector : MonoBehaviour
{
    public GameObject planetPrefab;
    [TextArea] public string planetInfo;
    public string planetName;

    void OnMouseDown()
    {
        PlanetDataHolder.Instance.selectedPlanetPrefab = planetPrefab;
        PlanetDataHolder.Instance.planetInfo = planetInfo;
        PlanetDataHolder.Instance.planetName = planetName;

        SceneManager.LoadScene("ARScene");
    }


}
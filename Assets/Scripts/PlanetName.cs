using UnityEngine;
using TMPro;

public class PlanetName : MonoBehaviour
{
    public string planetName;
    public Transform player;
    public GameObject uiText;

    public float showDistance = 15f;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < showDistance)
        {
            uiText.SetActive(true);
            uiText.GetComponent<TextMeshProUGUI>().text = planetName;
        }
        else
        {
            uiText.SetActive(false);
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetUI : MonoBehaviour
{
    public GameObject infoPanel;
    public TextMeshProUGUI planetNameText;
    public TextMeshProUGUI planetInfoText;
    public RectTransform content; 

    VerticalLayoutGroup layoutGroup;

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        planetNameText.text = PlanetDataHolder.Instance.planetName;
        planetInfoText.text = PlanetDataHolder.Instance.planetInfo;

        RefreshLayout();
    }

    void RefreshLayout()
    {
        layoutGroup = content.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            Destroy(layoutGroup);
        }
        StartCoroutine(ReAddLayout());
    }

    System.Collections.IEnumerator ReAddLayout()
    {
        yield return null; 

        layoutGroup = content.gameObject.AddComponent<VerticalLayoutGroup>();

        layoutGroup.childAlignment = TextAnchor.UpperCenter;
        layoutGroup.childControlHeight = true;
        layoutGroup.childControlWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = false;
    }
}
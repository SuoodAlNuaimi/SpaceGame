using UnityEngine;

public class PlantInteraction : MonoBehaviour
{
    public GameObject infoCanvas;

    public static GameObject selectedObject;

    void OnMouseDown()
    {
        Select();
    }

    public void Select()
    {
        selectedObject = gameObject;
        if (infoCanvas != null)
        {
            infoCanvas.SetActive(true);
        }
    }
}
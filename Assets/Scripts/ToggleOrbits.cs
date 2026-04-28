using UnityEngine;

public class ToggleOrbitsKey : MonoBehaviour
{
    public GameObject orbitParent;

    private bool isVisible = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // press O
        {
            isVisible = !isVisible;
            orbitParent.SetActive(isVisible);
        }
    }
}
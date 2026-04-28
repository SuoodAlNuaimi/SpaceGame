using UnityEngine;

public class PlanetLabel3D : MonoBehaviour
{
    public Transform player;
    public GameObject label;
    public float showDistance = 20f;

    void Start()
    {
        // Always start hidden
        if (label != null)
            label.SetActive(false);
    }

    void Update()
    {
        if (player == null || label == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        // ONLY distance logic
        bool shouldShow = distance <= showDistance;

        // Apply once (avoids spam)
        if (label.activeSelf != shouldShow)
            label.SetActive(shouldShow);

        // Face camera if visible
        if (shouldShow && Camera.main != null)
        {
            label.transform.LookAt(Camera.main.transform);
            label.transform.Rotate(0, 180, 0);
        }
    }
}
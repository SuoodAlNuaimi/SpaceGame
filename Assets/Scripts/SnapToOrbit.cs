using UnityEngine;

public class SnapToOrbit : MonoBehaviour
{
    public Transform sun;

    // MUST match your orbit line radii
    public float[] orbitRadii = { 12.5f, 18f, 24f, 33f, 53f, 67f, 78f, 93f };

    void OnMouseUp()
    {
        if (sun == null) return;

        // Direction from sun to planet
        Vector3 direction = (transform.position - sun.position).normalized;

        // Current distance
        float distance = Vector3.Distance(transform.position, sun.position);

        // Find closest orbit
        float closestRadius = orbitRadii[0];
        float minDiff = Mathf.Abs(distance - orbitRadii[0]);

        foreach (float r in orbitRadii)
        {
            float diff = Mathf.Abs(distance - r);

            if (diff < minDiff)
            {
                minDiff = diff;
                closestRadius = r;
            }
        }

        // Snap position
        Vector3 targetPosition = sun.position + direction * closestRadius;

        transform.position = targetPosition;
    }
}
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour
{
    public Transform center; // Sun
    public float speed = 10f;
    public float radius = 5f;
    public int segments = 100;

    private LineRenderer line;

    void Start()
    {
        // Set initial position based on radius
        transform.position = center.position + new Vector3(radius, 0, 0);

        // Setup LineRenderer
        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.loop = true;
        line.useWorldSpace = true;

        DrawOrbit();
    }

    void Update()
    {
        // Rotate around center
        transform.RotateAround(center.position, Vector3.up, speed * Time.deltaTime);
    }

    void DrawOrbit()
    {
        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 pos = new Vector3(x, 0, z) + center.position;
            line.SetPosition(i, pos);
        }
    }
}
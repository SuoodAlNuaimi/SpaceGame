using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OLine : MonoBehaviour
{
    public float radius = 25f;
    public int segments = 100;

    void Start()
    {
        LineRenderer line = GetComponent<LineRenderer>();

        line.positionCount = segments + 1;

        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
}
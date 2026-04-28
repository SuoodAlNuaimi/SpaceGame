using UnityEngine;

public class ShufflePlanets : MonoBehaviour
{
    public Transform sun;
    public Transform[] planets;

    // SAME radii as your orbits
    public float[] orbitRadii = { 12.5f, 18f, 24f, 33f, 53f, 67f, 78f, 93f };

    void Start()
    {
        Shuffle();
    }

        void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Shuffle();
        }
    }

    void Shuffle()
    {
        // Shuffle radii order
        for (int i = 0; i < orbitRadii.Length; i++)
        {
            float temp = orbitRadii[i];
            int randomIndex = Random.Range(i, orbitRadii.Length);
            orbitRadii[i] = orbitRadii[randomIndex];
            orbitRadii[randomIndex] = temp;
        }

        // Assign each planet a random orbit
        for (int i = 0; i < planets.Length; i++)
        {
            float radius = orbitRadii[i];

            // random angle around circle
            float angle = Random.Range(0f, 360f);

            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            planets[i].position = sun.position + new Vector3(x, 0, z);
        }
    }
}
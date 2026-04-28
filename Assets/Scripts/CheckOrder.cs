using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class CheckOrder : MonoBehaviour
{
    public Transform sun;

    public Transform[] planets;
    public TextMeshProUGUI Countext;
    // correct radii (same as orbits)
    public float[] correctRadii = { 12.5f, 18f, 24f, 33f, 53f, 67f, 78f, 93f };

    // correct planet order
    public string[] correctNames = {
        "Mercury", "Venus", "Earth", "Mars",
        "Jupiter", "Saturn", "Uranus", "Neptune"
    };

    public float tolerance = 2f; // allow small error

    public GameObject winPanel;
    public bool showGizmos = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckPlanets();
        }
    }


    public void CheckPlanets()
    {
        int correctCount = GetCorrectPlanetsCount();

        if (correctCount == planets.Length)
        {
            winPanel.SetActive(true);
            //Time.timeScale = 0f;
            Debug.Log("✅ Correct Order! YOU WIN 🎉");
            Countext.text = correctCount + " / " + planets.Length;
            if (QuestSystem.QuestManager.Instance.activeQuests[3].isCompleted == false)
            {
                QuestSystem.QuestManager.Instance.CompleteQuest("4");

            }
        }
        else
        {
            Debug.Log($"❌ Not quite! {correctCount} / {planets.Length} planets are in the correct place.");
            Countext.text = correctCount + " / " + planets.Length;
        }
    }
    
    /// <summary>
    /// Returns the number of planets currently in their correct orbit.
    /// </summary>
    public int GetCorrectPlanetsCount()
    {
        int count = 0;

        for (int i = 0; i < planets.Length; i++)
        {
            float distance = Vector3.Distance(planets[i].position, sun.position);

            // Check if distance is correct AND the planet name matches the expected index
            bool isCorrectDistance = Mathf.Abs(distance - correctRadii[i]) <= tolerance;
            bool isCorrectPlanet = planets[i].name == correctNames[i];

            if (isCorrectDistance && isCorrectPlanet)
            {
                count++;
            }
        }

        return count;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showGizmos || sun == null || correctRadii == null || correctNames == null) return;

        for (int i = 0; i < correctRadii.Length; i++)
        {
            // Set color for the orbits
            Gizmos.color = new Color(1f, 1f, 0f, 0.5f); // Semi-transparent yellow
            
            // Draw the orbit circle using Handles (which works in Scene View)
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.DrawWireDisc(sun.position, Vector3.up, correctRadii[i]);

            // Draw the name of the planet at the orbit edge
            string planetName = i < correctNames.Length ? correctNames[i] : "Unknown";
            Vector3 labelPos = sun.position + new Vector3(correctRadii[i], 0, 0);
            
            UnityEditor.Handles.BeginGUI();
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            UnityEditor.Handles.Label(labelPos, planetName, style);
            UnityEditor.Handles.EndGUI();
        }
    }
#endif
}
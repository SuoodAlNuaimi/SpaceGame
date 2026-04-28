using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Planet : MonoBehaviour
{
    [Header("Planet Identity")]
    public int planetIndex = 0;           // 0=Mercury, 1=Venus, ... 7=Neptune
    public string planetName = "Earth";

    public TextMeshProUGUI planetNameText;
    public Button startQuizButton;
    public Button closeButton;


    [Header("Approach Settings")]
    public float approachDistance = 8f;   // How close the ship must be to trigger
    public float rotationSpeed = 10f;     // Planets slowly rotate


    private bool playerInRange = false;
    private bool panelOpen = false;
    public Transform playerShip;

    public GameObject QuizPanel;
    public bool IsCompleted;

    public UnityEngine.Events.UnityEvent OnCLick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the player ship
        if (playerShip == null)
            playerShip = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        if (playerShip != null)
        {
            float dist = Vector3.Distance(transform.position, playerShip.position);
            bool inRange = dist <= approachDistance;

            if (inRange && !playerInRange) OnPlayerEnter();
            if (!inRange && playerInRange) OnPlayerExit();

            playerInRange = inRange;
        }
        Debug.Log(playerInRange);
        // Press E to open/close facts panel when in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (panelOpen)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
                if (OnCLick != null)
                {
                    OnCLick.Invoke();
                }
            }
        }
    }
    private void OnPlayerEnter()
    {
        Debug.Log($"Approaching {planetName}! Press E to learn more.");
        // You can also show a small "Press E" HUD hint here
    }

    private void OnPlayerExit()
    {
        if (panelOpen) ClosePanel();
    }
    public void OpenPanel()
    {
        Time.timeScale = 0;
        // Populate text fields
        if (planetNameText != null) planetNameText.text = planetName;
        // Wire up buttons
        if (startQuizButton != null)
            startQuizButton.onClick.AddListener(OnStartQuiz);
        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
        panelOpen = true;
        QuizPanel.SetActive(true);
        // Unlock cursor so player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePanel()
    {
        panelOpen = false;
        Time.timeScale = 1;
        QuizPanel.SetActive(false);

        // Re-lock cursor for flying
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ─── Quiz Trigger ─────────────────────────────────────────────────────────

    private void OnStartQuiz()
    {
        ClosePanel();
        Time.timeScale = 0;

        // Tell the QuizManager which planet we're quizzing
        QuizManager quiz = FindObjectOfType<QuizManager>();
        if (quiz != null)
        {
            quiz.StartQuiz(planetIndex);
        }
        else
        {
            Debug.LogWarning("QuizManager not found in scene!");
        }
    }
}

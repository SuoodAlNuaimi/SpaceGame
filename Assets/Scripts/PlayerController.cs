using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;
    public float mouseSensitivity = 100f;

    [Header("Boost Settings")]
    public float boostMultiplier = 2.5f;
    public float boostDuration = 1.0f;
    private float boostTimer = 0f;
    private bool isBoosting = false;

    float xRotation = 0f;

    public bool isEditing = false;
    public QuestSystem.QuestSystemUI UI;

    void Start()
    {
        UnlockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnComplete()
    {
        Invoke("UnlockCursor",0.5f);
        isEditing = true;
    }
    void Update()
    {

        // Toggle mode
        if (Input.GetKeyDown(KeyCode.Tab)&& !UI.questPanel.activeInHierarchy)
        {
            isEditing = !isEditing;

            if (isEditing)
                UnlockCursor();
            else
                LockCursor();
        }

        // 🚫 Stop movement when editing
        if (isEditing)
            return;


        // MOUSE LOOK
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // MOVEMENT INPUT
        float forward = Input.GetAxis("Vertical");   // W/S
        float side = Input.GetAxis("Horizontal");    // A/D

        // Calculate Roll during boost
        float zRoll = 0f;
        if (isBoosting)
        {
            float rollProgress = 1f - (boostTimer / boostDuration);
            zRoll = rollProgress * 360f;
        }

        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y + mouseX, zRoll);

        // Handle Boost
        if (Input.GetKeyDown(KeyCode.Space) && !isBoosting && forward > 0)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            if (QuestSystem.QuestManager.Instance)
            {
                if (QuestSystem.QuestManager.Instance.activeQuests[2].isCompleted == false)
                {
                    QuestSystem.QuestManager.Instance.CompleteQuest("3");


                }
            }
            
        }

        float currentSpeed = speed;
        if (isBoosting)
        {
            currentSpeed *= boostMultiplier;
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                isBoosting = false;
            }
        }

        Vector3 move = transform.forward * forward + transform.right * side;
        transform.position += move * currentSpeed * Time.deltaTime;
    }
}
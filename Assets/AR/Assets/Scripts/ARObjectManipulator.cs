using UnityEngine;

public class ARObjectManipulator : MonoBehaviour
{
    public static GameObject selectedObject;

    public float rotationSpeed = 0.2f;
    public float autoRotateSpeed = 10f; 
    public float minScale = 0.1f;
    public float maxScale = 0.5f;

    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        if (selectedObject == null)
            return;

        AutoRotate();       
        HandleRotation();  
        HandleScaling();
    }

    void AutoRotate()
    {
        // slow rotation on Y axis
        selectedObject.transform.Rotate(0, autoRotateSpeed * Time.deltaTime, 0);
    }

    void HandleRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float rotX = touch.deltaPosition.y * rotationSpeed; 
                float rotY = touch.deltaPosition.x * rotationSpeed; 

                selectedObject.transform.Rotate(rotX, -rotY, 0);
            }
        }
    }

    void HandleScaling()
    {
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t1.position, t2.position);

            if (t2.phase == TouchPhase.Began)
            {
                initialDistance = currentDistance;
                initialScale = selectedObject.transform.localScale;
            }
            else
            {
                if (Mathf.Approximately(initialDistance, 0))
                    return;

                float factor = currentDistance / initialDistance;
                Vector3 newScale = initialScale * factor;

                float clamped = Mathf.Clamp(newScale.x, minScale, maxScale);
                selectedObject.transform.localScale =
                    new Vector3(clamped, clamped, clamped);
            }
        }
    }
}
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 12f;
    public float height = 5f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        // Position behind the ship
        Vector3 desiredPosition = target.position 
                                - target.forward * distance 
                                + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Look at the ship
        transform.LookAt(target.position + Vector3.up * 2f);
    }
}
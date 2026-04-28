using UnityEngine;

public class DragPlanet : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;

    private float initialY;

    void OnMouseDown()
    {
        initialY = transform.position.y;
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        Vector3 newPos = GetMouseWorldPos() + offset;
        newPos.y = initialY; // Lock the Y position to the initial height
        transform.position = newPos;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
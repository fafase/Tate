using UnityEngine;

public class InputDetection : InputBase
{
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
        }
    }

    private void DetectClick()
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;
            clickedObject.GetComponent<IClickable>()?.OnPress();
        }
    }
}

public interface IClickable 
{
    void OnPress();
}

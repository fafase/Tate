using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetection : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Detect if the user has clicked or tapped
        if (Input.GetMouseButtonDown(0)) // 0 = Left Mouse Button
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

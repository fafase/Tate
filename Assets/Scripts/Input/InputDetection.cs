using UnityEngine;

namespace Tatedrez.Input
{
    public class InputDetection : InputBase
    {
        private Camera mainCamera;
        private void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                DetectClick();
            }
        }

        private void DetectClick()
        {
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                clickedObject.GetComponent<IClickable>()?.OnPress();
            }
        }
    }
}
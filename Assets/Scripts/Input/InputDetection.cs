using Tatedrez.Core;
using UnityEngine;
using Zenject;

public class InputDetection : MonoBehaviour
{
    [Inject] private ICoreController m_core;

    [SerializeField] private Turn m_turn;

    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        m_core
            .CurrentTurn
            .Subscribe(turn => enabled = turn == m_turn);
        enabled = m_turn == Turn.Player1;
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

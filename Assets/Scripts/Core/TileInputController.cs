using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(SpriteRenderer))]
public class TileInputController : MonoBehaviour, IClickable, ITile
{   
    [Inject] private ICoreController m_core;
    private SpriteRenderer m_spriteRenderer;
    public bool IsAvailable { get; private set; } = true;
    public Vector3 Position => transform.position;

    void Start() 
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.enabled = false;
    }
    public void OnPress()
    {
        if(m_core.SelectedPawn != null && IsAvailable) 
        {
            IsAvailable = false;
            m_core.MoveSelectedToPosition(this);  
        }
    }

    public void FreeTile() => IsAvailable = true;
}

public interface ITile 
{
    bool IsAvailable {  get; }
    Vector3 Position {  get; }
    void FreeTile();
}

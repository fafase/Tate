using System;
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
    public string Name => gameObject.name;
    public IPawn CurrentPawn { get; private set; }

    public int GridX { get; private set; }
    public int GridY { get; private set; }

    void Start() 
    {
        GenerateGridPosition();
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

    private void GenerateGridPosition() 
    {
        string[] splits = name.Split('_');
        if (splits.Length != 2) 
        {
            throw new System.Exception("Issue with tile name, should be x_y");
        }
        if (!Int32.TryParse(splits[0], out int x)) 
        {
            throw new System.Exception("Issue with tile name, should be x_y");
        }
        if (!Int32.TryParse(splits[1], out int y))
        {
            throw new System.Exception("Issue with tile name, should be x_y");
        }
        GridX = x;
        GridY = y;
    }
}

public interface ITile 
{
    string Name { get; }
    bool IsAvailable {  get; }
    Vector3 Position {  get; }
    int GridX { get; }
    int GridY { get; }
    void FreeTile();
    IPawn CurrentPawn { get; }
}

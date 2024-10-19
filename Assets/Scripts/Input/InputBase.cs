using Tatedrez.Core;
using UnityEngine;
using Zenject;

public class InputBase : MonoBehaviour
{
    [SerializeField] private InputType m_inputType;
    [SerializeField] private Turn m_turn;
    [Inject] private ICoreController m_core;

    public InputType InputType => m_inputType;
    public Turn Turn => m_turn;

    protected virtual void Start() 
    {
        m_core
        .CurrentTurn
        .Subscribe(turn => enabled = turn == m_turn);
        enabled = m_core.CurrentTurn.Value == m_turn;
    }
}
public enum InputType 
{
    Player, NPC
}

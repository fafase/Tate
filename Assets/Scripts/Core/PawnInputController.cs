using UnityEngine;
using Zenject;

public class PawnInputController : MonoBehaviour, IClickable, IPawn
{
    [Inject] private ICoreController m_core;
    [SerializeField] private Turn m_item;
    [SerializeField] private GameObject m_background;
    [SerializeField] private PawnType m_pawnType;

    private static PawnInputController m_selected;
    private Turn m_current;
    public ITile CurrentTile { get; private set; }
    public bool HasMovedToDeck {  get; private set; }
    public PawnType PawnType => m_pawnType;

    public Turn PawnTurn => m_item;

    void Start() 
    {
        m_core
            .CurrentTurn
            .Subscribe(turn => m_current = turn);
    }

    public void OnPress()
    {
        if (m_item != m_current)
        {
            return;
        }
        if (HasMovedToDeck && !m_core.AllPawnsOnDeck)
        {
            return;
        }
        if (m_selected != null && m_selected != this)
        {
            m_selected.SetBackground(false);
        }
        HasMovedToDeck = true;
        m_selected = this;
        m_core.SetSelectedPawn(this);
        SetBackground(!m_background.activeSelf);
    }


    private void SetBackground(bool value) 
    {
        m_background.SetActive(value);
    }
    public void MoveToPosition(ITile tile) 
    {
        CurrentTile?.FreeTile();
        CurrentTile = tile;
        Vector3 position = CurrentTile.Position;
        position.z = -1f;
        transform.position = position;
        SetBackground(false);
    }
}
public enum PawnType 
{
    Horse, Tower, Bishop
}
public interface IPawn
{
    void MoveToPosition(ITile tile);
    ITile CurrentTile { get; }
    bool HasMovedToDeck { get; }
    Turn PawnTurn {  get; }
    PawnType PawnType { get;}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.Core;
using Tools;
using UnityEngine;
using Zenject;

public class InputService : MonoBehaviour
{
    [Inject] private ICoreController m_core;
    private List<InputBase> m_inputs;
    private InputBase m_current;

    private void Start()
    {
        m_inputs = GetComponents<InputBase>().ToList();

        string value = PlayerPrefs.GetString(Player.Key);
        Predicate<InputBase> predicate = null;
        switch (value) 
        {
            case Player.Single:
                predicate = new Predicate<InputBase>(input => 
                    input.InputType.Equals(InputType.Player) && input.Turn.Equals(Turn.Player2));
                break;
            case Player.Two:
                predicate = new Predicate<InputBase>(input => input.InputType.Equals(InputType.NPC));
                break;
        }
        SetInputs(predicate);
        Signal.Connect<EndGameSignal>(OnEndGame);
        Signal.Connect<PawnMovementSignal>(OnPawnMovement);
        Signal.Connect<PauseGameSignal>(OnPause);

        m_core
           .CurrentTurn
           .Subscribe(SetCurrentInput);

        SetCurrentInput(m_core.CurrentTurn.Value);
    }

    private void OnDestroy()
    {
        Signal.Disconnect<EndGameSignal>(OnEndGame);
        Signal.Disconnect<PawnMovementSignal>(OnPawnMovement);
        Signal.Disconnect<PauseGameSignal>(OnPause);
    }

    private void SetCurrentInput(Turn turn) 
    {
        m_inputs.ForEach(input => 
        { 
            input.enabled = turn == input.Turn;
            if (input.enabled) 
            {
                m_current = input;
            }
        });
    }

    private void OnEndGame() 
    {
        m_inputs.ForEach(input => input.enabled = false);    
    }

    private void OnPawnMovement(PawnMovementSignal data) 
    {
        if (data.StartMovement)
        {
            m_inputs.ForEach(input => input.enabled = false);
        }
    }

    private void SetInputs(Predicate<InputBase> predicate) 
    {
        var input = m_inputs.Find(input => predicate(input));
        if (input != null) input.enabled = false;
        m_inputs.Remove(input);
    }

    private void OnPause(PauseGameSignal data)
    {
        m_current.enabled = !data.IsPaused;
    }
}

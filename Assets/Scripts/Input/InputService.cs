using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.Core;
using Tools;
using UnityEngine;

public class InputService : MonoBehaviour
{
    private List<InputBase> m_inputs;
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
    }

    private void OnDestroy()
    {
        Signal.Disconnect<EndGameSignal>(OnEndGame);
        Signal.Disconnect<PawnMovementSignal>(OnPawnMovement);
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
}

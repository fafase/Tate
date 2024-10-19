using System.Collections.Generic;
using System.Linq;
using Tatedrez.Core;
using UnityEngine;

public class InputService : MonoBehaviour
{ 
    private void Start()
    {
        List<InputBase> inputs = GetComponents<InputBase>().ToList();

        string value = PlayerPrefs.GetString(Player.Key);
        switch (value) 
        {
            case Player.Single:
                SetPlayerVsAI(inputs);
                break;
            case Player.Two:
                SetTwoPlayers(inputs);
                break;
        }
    }

    private void SetPlayerVsAI(List<InputBase> inputs) 
    {
        var player2 = inputs.Find(input => input.InputType.Equals(InputType.Player) && input.Turn.Equals(Turn.Player2));
        if (player2 != null) player2.enabled = false;
    }
    private void SetTwoPlayers(List<InputBase> inputs ) 
    {
        var npc = inputs.Find(input => input.InputType.Equals(InputType.NPC));
        if(npc != null) npc.enabled = false;
       
    }
}

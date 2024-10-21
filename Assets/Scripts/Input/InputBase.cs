using Tatedrez.Core;
using UnityEngine;

namespace Tatedrez.Input
{
    public class InputBase : MonoBehaviour
    {
        [SerializeField] private InputType m_inputType;
        [SerializeField] private Turn m_turn;

        public InputType InputType => m_inputType;
        public Turn Turn => m_turn;
        public virtual void StopInput()
        {
            enabled = false;
        }
    }
    public enum InputType
    {
        Player, NPC
    }
}

using System;
using Tools;
using UnityEngine;
using Zenject;
using Tatedrez.Input;

namespace Tatedrez.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileInputController : MonoBehaviour, IClickable, ITile
    {
        [Inject] private ICoreController m_core;
        private SpriteRenderer m_availableTile;
        public bool IsAvailable { get; private set; } = true;
        public Vector3 Position => transform.position;
        public string Name => gameObject.name;
        public IPawn CurrentPawn { get; set; }

        public int GridX { get; private set; }
        public int GridY { get; private set; }


        void Start()
        {
            GenerateGridPosition();
            m_availableTile = GetComponent<SpriteRenderer>();
            m_availableTile.enabled = false;
        }
        public void OnPress()
        {
            Signal.Send(new AudioSignal(AudioSignal.Tap));
            if (!m_core.AllPawnsOnDeck) 
            {
                if (m_core.SelectedPawn != null && IsAvailable)
                {
                    IsAvailable = false;
                    m_core.MoveSelectedToPosition(this);
                }
                return;
            }
            if (m_availableTile.enabled) 
            {
                IsAvailable = false;
                m_core.MoveSelectedToPosition(this);
                m_availableTile.enabled = false;
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

        public void SetTileBackground(bool state) 
        {
            m_availableTile.enabled = state;
        }
    }

    public interface ITile
    {
        string Name { get; }
        bool IsAvailable { get; }
        Vector3 Position { get; }
        int GridX { get; }
        int GridY { get; }
        void FreeTile();
        IPawn CurrentPawn { get; set; }
        void SetTileBackground(bool state);
    }
}
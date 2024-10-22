using Tools;
using Zenject;
using System;
using UnityEngine;

namespace Tatedrez.Core
{
    public class GridController : IGrid, IInitializable, IDisposable
    {
        [Inject] private ICoreController m_core;

        public IPawn[,] Grid { get; private set; } = new IPawn[3, 3];
        public ITile[,] Tiles { get; private set; } = new ITile[3, 3];

        private bool m_isDisposed = false;

        public void Initialize()
        {
            Signal.Connect<PawnMovementSignal>(OnPawnMovement);
        }

        public void Dispose() 
        {
            if(m_isDisposed) return;
            m_isDisposed = true;
            Signal.Disconnect <PawnMovementSignal>(OnPawnMovement);
            Grid = null;
        }
        public void InitWithController(ICoreController core) 
        {
            m_core = core;
        }

        public void GenerateTileGrid(GameObject tileContainer)
        {
            var tiles = tileContainer.GetComponentsInChildren<ITile>(true);
            foreach (ITile tile in tiles)
            {
                Tiles[tile.GridX, tile.GridY] = tile;
            }
        }

        public (int row, int col)? FindInstance(IPawn instance)
        {
            for (int row = 0; row < Grid.GetLength(0); row++)
            {
                for (int col = 0; col < Grid.GetLength(1); col++)
                {
                    if (ReferenceEquals(Grid[row, col], instance))
                    {
                        return (row, col);
                    }
                }
            }
            return null;
        }

        public void CheckWin(Turn turn)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((Grid[i, 0]?.Owner == turn && Grid[i, 1]?.Owner == turn && Grid[i, 2]?.Owner == turn) || 
                    (Grid[0, i]?.Owner == turn && Grid[1, i]?.Owner == turn && Grid[2, i]?.Owner == turn))   
                {
                    m_core.SetWin(turn);
                }
            }

            if ((Grid[0, 0]?.Owner == turn && Grid[1, 1]?.Owner == turn && Grid[2, 2]?.Owner == turn) || 
                (Grid[0, 2]?.Owner == turn && Grid[1, 1]?.Owner == turn && Grid[2, 0]?.Owner == turn))   
            {
                m_core.SetWin(turn);
            }
        }

        public void SetPawnOnGrid(IPawn pawn, int x, int y) => Grid[x, y] = pawn;

        public void Reset()
        {
            for (int row = 0; row < Grid.GetLength(0); row++)
            {
                for (int col = 0; col < Grid.GetLength(1); col++)
                {
                    Grid[row, col] = null;
                }
            }
        }

        private void OnPawnMovement(PawnMovementSignal movement)
        {
            if (movement.StartMovement)
            {
                return;
            }
            IPawn pawn = movement.Pawn;
            var pos = FindInstance(movement.Pawn);
            if (pos != null)
            {
                Grid[pos.Value.row, pos.Value.col] = null;
            }
            ITile tile = movement.Tile;
            Grid[tile.GridX, tile.GridY] = movement.Pawn;
            CheckWin(pawn.Owner);
        }
    }

    public interface IGrid
    {
        IPawn[,] Grid { get; }
        ITile[,] Tiles { get; }
        void GenerateTileGrid(GameObject tileContainer);
    }
}


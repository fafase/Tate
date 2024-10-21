using Tools;
using Zenject;
using System;

namespace Tatedrez.Core
{
    public class GridController : IGrid, IInitializable, IDisposable
    {
        [Inject] private ICoreController m_core;
        private IPawn[,] m_grid = new IPawn[3, 3];
        private bool m_isDisposed = false;
        public IPawn[,] Grid => m_grid;

        public void Initialize()
        {
            Signal.Connect<PawnMovementSignal>(OnPawnMovement);
        }
        public void Dispose() 
        {
            if(m_isDisposed) return;
            m_isDisposed = true;
            Signal.Disconnect <PawnMovementSignal>(OnPawnMovement);
            m_grid = null;
        }
        public void InitWithController(ICoreController core) 
        {
            m_core = core;
        }


        private void OnPawnMovement(PawnMovementSignal movement)
        {
            if(movement.StartMovement)
            {
                return;
            }
            IPawn pawn = movement.Pawn;
            var pos = FindInstance(movement.Pawn);
            if (pos != null)
            {
                m_grid[pos.Value.row, pos.Value.col] = null;
            }
            ITile tile = movement.Tile;
            m_grid[tile.GridX, tile.GridY] = movement.Pawn;
            CheckWin(pawn.PawnTurn);
        }

        public (int row, int col)? FindInstance(IPawn instance)
        {
            for (int row = 0; row < m_grid.GetLength(0); row++)
            {
                for (int col = 0; col < m_grid.GetLength(1); col++)
                {
                    if (ReferenceEquals(m_grid[row, col], instance))
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
                if ((m_grid[i, 0]?.PawnTurn == turn && m_grid[i, 1]?.PawnTurn == turn && m_grid[i, 2]?.PawnTurn == turn) || 
                    (m_grid[0, i]?.PawnTurn == turn && m_grid[1, i]?.PawnTurn == turn && m_grid[2, i]?.PawnTurn == turn))   
                {
                    m_core.SetWin(turn);
                }
            }

            if ((m_grid[0, 0]?.PawnTurn == turn && m_grid[1, 1]?.PawnTurn == turn && m_grid[2, 2]?.PawnTurn == turn) || 
                (m_grid[0, 2]?.PawnTurn == turn && m_grid[1, 1]?.PawnTurn == turn && m_grid[2, 0]?.PawnTurn == turn))   
            {
                m_core.SetWin(turn);
            }
        }

        public void SetPawnOnGrid(IPawn pawn, int x, int y) => m_grid[x, y] = pawn;

        public void Reset()
        {
            for (int row = 0; row < m_grid.GetLength(0); row++)
            {
                for (int col = 0; col < m_grid.GetLength(1); col++)
                {
                    m_grid[row, col] = null;
                }
            }
        }
    }

    public interface IGrid
    {
        IPawn[,] Grid { get; }
    }
}


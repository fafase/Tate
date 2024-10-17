public class GridController 
{
    private IPawn[,] m_grid = new IPawn[3, 3];
    private ICoreController m_core;

    public void Init(ICoreController core) 
    {
        m_core = core;
        m_core.LastMovement.Subscribe(OnPawnMovement);
    }

    private void OnPawnMovement(Movement movement)
    {
        IPawn pawn = movement.pawn;
        var pos = FindInstance(movement.pawn);
        if(pos != null) 
        {
            m_grid[pos.Value.row, pos.Value.col] = null;
        }
        ITile tile = movement.tile;
        m_grid[tile.GridX, tile.GridY] = movement.pawn;
        CheckWin(pawn.PawnTurn);
    }

    public (int row, int col) ? FindInstance(IPawn instance) 
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
        // Check rows and columns
        for (int i = 0; i < 3; i++)
        {
            if ((m_grid[i, 0]?.PawnTurn == turn && m_grid[i, 1]?.PawnTurn == turn && m_grid[i, 2]?.PawnTurn == turn) || // Check row
                (m_grid[0, i]?.PawnTurn == turn && m_grid[1, i]?.PawnTurn == turn && m_grid[2, i]?.PawnTurn == turn))   // Check column
            {
                m_core.SetWin(turn);
            }
        }

        // Check diagonals
        if ((m_grid[0, 0]?.PawnTurn == turn && m_grid[1, 1]?.PawnTurn == turn && m_grid[2, 2]?.PawnTurn == turn) || // Check main diagonal
            (m_grid[0, 2]?.PawnTurn == turn && m_grid[1, 1]?.PawnTurn == turn && m_grid[2, 0]?.PawnTurn == turn))   // Check anti-diagonal
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


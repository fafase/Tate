using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using Tatedrez.Core;

public class PawnMovementTest
{
    protected PawnMovement m_move;
    protected ICoreController m_core;
    protected IGrid m_grid;
    protected IPawn m_pawn;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        m_core = Substitute.For<ICoreController>();
        m_grid = Substitute.For<IGrid>();
        m_move = new PawnMovement(m_core, m_grid);
        m_pawn = Substitute.For<IPawn>();
    }

    protected void SetPawn(PawnType type, int x, int y)
    {
        m_pawn.PawnType.Returns(type);
        ITile tile = Substitute.For<ITile>();
        tile.GridX.Returns(x); tile.GridY.Returns(y);
        m_pawn.CurrentTile.Returns(tile);
    }

    protected IPawn[,] CreateGrid(int size, int row, int col)
    {
        var grid = new IPawn[size, size];
        grid[row, col] = m_pawn;
        m_grid.Grid.Returns(grid);
        return grid;
    }

    protected void SetupWithTestCases(PawnType type, int x, int y, List<(int row, int col)> pawns) 
    {
        m_core.AllPawnsOnDeck.Returns(true);
        SetPawn(type, x, y);
        var grid = CreateGrid(3, x, y);
        pawns.ForEach(p => grid[p.row, p.col] = Substitute.For<IPawn>());
    }
}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tatedrez.Core;
using NSubstitute;
using System.Drawing;

public class PotentialMoveTest : PawnMovementTest
{
    private List<IPawn> m_pawns;

    [Test]
    public void Potential_NoMove() 
    {
        TestSetup(2);

        bool result = m_move.HasPotentialMove(m_pawns, Turn.Player1);
        Assert.IsFalse(result);
    }

    [Test]
    public void Potential_HasMove()
    {
        TestSetup(0);

        bool result = m_move.HasPotentialMove(m_pawns, Turn.Player1);
        Assert.IsTrue(result);
    }

    private void TestSetup(int y) 
    {
        var grid = new IPawn[3, 3];
        m_grid.Grid.Returns(grid);

        m_pawns = new List<IPawn>() {
            CreatePawn(PawnType.Horse, 1, 1),
            CreatePawn(PawnType.Bishop, 0, 0),
            CreatePawn(PawnType.Tower, 0, 2)
        };

        new List<(int row, int col)>
            { (0, 1), (1, 2), (2, y) }
            .ForEach((pos) => grid[pos.row, pos.col] = Substitute.For<IPawn>());
    }

    private IPawn CreatePawn(PawnType type, int x, int y)
    {
        IPawn pawn = Substitute.For<IPawn>();
        ITile tile = Substitute.For<ITile>();
        tile.GridX.Returns(x); tile.GridY.Returns(y);
        pawn.CurrentTile.Returns(tile);
        pawn.PawnTurn.Returns(Turn.Player1);
        pawn.PawnType.Returns(type);
        m_grid.Grid[x,y] = pawn;
        return pawn;
    }
}

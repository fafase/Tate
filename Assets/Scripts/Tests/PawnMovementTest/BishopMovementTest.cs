using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Tatedrez.Core;

public class BishopMovementTest : PawnMovementTest
{
    public static IEnumerable TestCases => new[]
{
        new object[] { 0, 0, new List<(int row, int col)> { (0, 1) },  new List<(int row, int col)> { (1, 1), (2, 2)} },
        new object[] { 0, 0, new List<(int row, int col)> { (0, 1), (2, 2) },  new List<(int row, int col)> { (1, 1) } },
        new object[] { 0, 0, new List<(int row, int col)> { (1, 1), (2, 2), (0, 1) },  new List<(int row, int col)> { } },

        new object[] { 0, 1, new List<(int row, int col)> { (0, 0) },  new List<(int row, int col)> { (1, 0), (1, 2)} },
        new object[] { 0, 1, new List<(int row, int col)> { (0, 0), (1, 0) },  new List<(int row, int col)> { (1, 2)} },
        new object[] { 0, 1, new List<(int row, int col)> { (0, 0), (1, 0), (1, 2) },  new List<(int row, int col)> { } },

        new object[] { 1, 1, new List<(int row, int col)> { (0, 1) },  new List<(int row, int col)> { (0, 0), (0, 2), (2, 0), (2, 2)} },
        new object[] { 1, 1, new List<(int row, int col)> { (0, 1), (0, 0), (0, 2) },  new List<(int row, int col)> { (2, 0), (2, 2)} },
        new object[] { 1, 1, new List<(int row, int col)> { (0, 1), (0, 0), (0, 2), (2, 0), (2, 2) },  new List<(int row, int col)> { } },

        new object[] { 2, 1, new List<(int row, int col)> { (0, 0) },  new List<(int row, int col)> { (1, 0), (1, 2)} },
        new object[] { 2, 1, new List<(int row, int col)> { (0, 0), (1, 0) },  new List<(int row, int col)> { (1, 2)} },
        new object[] { 2, 1, new List<(int row, int col)> { (0, 0), (1, 0), (1, 2) },  new List<(int row, int col)> { } },
    };

    [Test, TestCaseSource(nameof(BishopMovementTest.TestCases))]
    public void Bishop_Tests(int x, int y, List<(int row, int col)> pawns, List<(int row, int col)> expected)
    {
        SetupWithTestCases(PawnType.Bishop, x, y, pawns);

        List<(int row, int col)> result = m_move.CheckForAllowedMoves(m_pawn);

        CollectionAssert.AreEquivalent(expected, result); 
    }
}
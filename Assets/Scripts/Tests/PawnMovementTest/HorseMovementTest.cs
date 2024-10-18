using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez.Core;

public class HorseMovementTest : PawnMovementTest
{
    public static IEnumerable TestCasesEmptyDeck => new[] 
    {
        new object[] { 0, 0, new List<(int row, int col)> { }, new List<(int row, int col)> { (1, 2), (2, 1)} },
        new object[] { 0, 1, new List<(int row, int col)> { }, new List<(int row, int col)> { (2, 0), (2, 2)} },
        new object[] { 0, 2, new List<(int row, int col)> { }, new List<(int row, int col)> { (1, 0), (2, 1)} },
        
        new object[] { 1, 0, new List<(int row, int col)> { }, new List<(int row, int col)> { (0, 2), (2, 2)} },
        new object[] { 1, 1, new List<(int row, int col)> { }, new List<(int row, int col)>() },
        new object[] { 1, 2, new List<(int row, int col)> { }, new List<(int row, int col)> { (0, 0), (2, 0)} },

        new object[] { 2, 0, new List<(int row, int col)> { }, new List<(int row, int col)> { (0, 1), (1, 2)} },
        new object[] { 2, 1, new List<(int row, int col)> { }, new List<(int row, int col)> { (0, 0), (0, 2)} },
        new object[] { 2, 2, new List<(int row, int col)> { }, new List<(int row, int col)> { (1, 0), (0, 1)} },
    };

    [Test, TestCaseSource(nameof(HorseMovementTest.TestCasesEmptyDeck))]
    public void Horse_Test(int x, int y, List<(int row, int col)>pawns, List<(int row, int col)> expected)
    {
        SetupWithTestCases(PawnType.Horse, x, y, pawns);

        List<(int row, int col)> result = m_move.CheckForAllowedMoves(m_pawn);
        CollectionAssert.AreEquivalent(expected, result);
    }
}

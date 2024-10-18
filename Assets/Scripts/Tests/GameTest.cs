using NSubstitute;
using NUnit.Framework;

public class GameTest
{
    private GridController m_grid;
    private ICoreController m_coreController;
    [OneTimeSetUp]
    public void OneTimeSetUp() 
    {
        m_coreController = Substitute.For<ICoreController>();
        m_grid = new GridController(m_coreController);
    }

    [SetUp]
    public void SetUp() 
    {
        m_grid.Reset();
        m_coreController.ClearReceivedCalls();
    }

    [Test]
    public void GridControlFindInstance() 
    {
        IPawn pawnA = Substitute.For<IPawn>();
        m_grid.SetPawnOnGrid(pawnA, 0, 1);
        IPawn pawnB = Substitute.For<IPawn>();
        m_grid.SetPawnOnGrid(pawnB, 1, 1);

        var result = m_grid.FindInstance(pawnA);

        Assert.IsNotNull(result);
        Assert.AreEqual(result.Value.row, 0);
        Assert.AreEqual(result.Value.col, 1);
    }

    [Test]
    public void GridControlFindNoInstance()
    {
        IPawn pawnA = Substitute.For<IPawn>();
        IPawn pawnB = Substitute.For<IPawn>();
        m_grid.SetPawnOnGrid(pawnB, 1, 1);

        var result = m_grid.FindInstance(pawnA);

        Assert.IsNull(result);
    }

    [Test]
    public void GridControlCheckNoWin()
    {
        CreatePawn(Turn.Player1, 0, 0);
        CreatePawn(Turn.Player1, 0, 1);
        CreatePawn(Turn.Player1, 1, 0);

        CreatePawn(Turn.Player2, 1, 1);
        CreatePawn(Turn.Player2, 2, 0);
        CreatePawn(Turn.Player2, 2, 1);

        m_grid.CheckWin(Turn.Player1);
        m_coreController.DidNotReceive().SetWin(Turn.Player1);
    }

    [Test]
    public void GridControlCheckWinRow()
    {
        CreatePawn(Turn.Player1, 0, 0);
        CreatePawn(Turn.Player1, 0, 1);
        CreatePawn(Turn.Player1, 0, 2);

        CreatePawn(Turn.Player2, 1, 1);
        CreatePawn(Turn.Player2, 2, 0);
        CreatePawn(Turn.Player2, 2, 1);

        m_grid.CheckWin(Turn.Player1);
        m_coreController.Received().SetWin(Turn.Player1);
    }

    [Test]
    public void GridControlCheckWinDiagonal()
    {
        CreatePawn(Turn.Player1, 0, 0);
        CreatePawn(Turn.Player1, 1, 1);
        CreatePawn(Turn.Player1, 2, 2);

        CreatePawn(Turn.Player2, 0, 1);
        CreatePawn(Turn.Player2, 2, 0);
        CreatePawn(Turn.Player2, 2, 1);

        m_grid.CheckWin(Turn.Player1);
        m_coreController.Received().SetWin(Turn.Player1);
    }


    [Test]
    public void GridControlCheckCoreNotification()
    {
        CreatePawn(Turn.Player1, 0, 0);
        CreatePawn(Turn.Player1, 1, 1);
        CreatePawn(Turn.Player1, 2, 2);

        CreatePawn(Turn.Player2, 0, 1);
        CreatePawn(Turn.Player2, 2, 0);
        CreatePawn(Turn.Player2, 2, 1);

        m_grid.CheckWin(Turn.Player1);
        m_coreController.Received().SetWin(Turn.Player1);
    }

    private void CreatePawn(Turn turn, int x, int y)
    {
        IPawn pawn = Substitute.For<IPawn>();
        pawn.PawnTurn.Returns(turn);
        m_grid.SetPawnOnGrid(pawn, x, y);
    }
}

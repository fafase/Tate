using NSubstitute;
using NUnit.Framework;
using Tatedrez.Core;

public class GameTest
{
    private GridController m_grid;
    private ICoreController m_coreController;
    [OneTimeSetUp]
    public void OneTimeSetUp() 
    {
        m_coreController = Substitute.For<ICoreController>();
        m_grid = new GridController();
        m_grid.InitWithController(m_coreController);
    }

    [SetUp]
    public void SetUp() 
    {
        m_grid.Reset();
        m_coreController.ClearReceivedCalls();
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
        pawn.Owner.Returns(turn);
        m_grid.SetPawnOnGrid(pawn, x, y);
    }
}

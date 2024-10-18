using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tatedrez.Core
{
    public class MovementController
    {
        private IGrid m_grid;
        private PawnMovement m_pawnMovement;
        public MovementController(IGrid grid)
        {
            m_grid = grid;
            m_pawnMovement = new PawnMovement(m_grid);
        }

        public void CheckForMovement(IPawn pawn)
        {
            List<(int row, int col)> result = m_pawnMovement.CheckForAllowedMoves(pawn);

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController 
{
    private IGrid m_grid;
    public MovementController(IGrid grid)
    {
        m_grid = grid;
    }

    public void CheckForMovement(IPawn pawn)
    {
        for (int i = 0; i < 3; i++)
        { 
            for (int j = 0; j < 3; j++) 
            {
                if (m_grid.Grid[i, j] == null) 
                {
                    Debug.Log($"Available at {i}{j}");
                }
            }
        }
    }
}

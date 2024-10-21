using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tatedrez.Core
{
    public class PawnMovement
    {
        private IGrid m_grid;
        private ICoreController m_core;
        public bool JumpOver { get; set; } = false;
        public PawnMovement(ICoreController core, IGrid grid)
        {
            m_core = core;
            m_grid = grid;
        }

        public List<(int row, int col)> CheckForAllowedMoves(IPawn pawn)
        {
            if (!m_core.AllPawnsOnDeck)
            {
                List<(int row, int col)> list = new();
                int size = m_grid.Grid.GetLength(0);
                for(int x = 0; x < size; x++) 
                {
                    for(int y = 0; y < size; y++) 
                    {
                        if (m_grid.Grid[x, y] == null) 
                        {
                            list.Add((x, y));
                        }
                    }
                }
                return list;
            }
            switch (pawn.PawnType)
            {
                case PawnType.Tower:
                    return JumpOver ? TowerMovementOver(pawn.CurrentTile) : TowerMovement(pawn.CurrentTile);
                case PawnType.Bishop:
                    return JumpOver ? BishopMovementOver(pawn.CurrentTile) : BishopMovement(pawn.CurrentTile);
                case PawnType.Horse:
                    return HorseMovement(pawn.CurrentTile);
            }
            return null;
        }

        private (HashSet<(int row, int col)>, int x, int y, IPawn[,] grid, int size) Setup(ITile tile) 
        {
            IPawn[,] grid = m_grid.Grid;
            int size = grid.GetLength(0);
            return (new HashSet<(int row, int col)>(), tile.GridX, tile.GridY, grid, grid.GetLength(0));
        }

        private List<(int row, int col)> TowerMovementOver(ITile tile)
        {
            var(tiles, x, y, grid, size) = Setup(tile);

            for (int i = 0; i < size; i++)
            {
                if (grid[x, i] == null)
                {
                    tiles.Add((x, i));
                }
                if (grid[i, y] == null)
                {
                    tiles.Add((i, y));
                }
            }

            return tiles.ToList();
        }

        private List<(int row, int col)> TowerMovement(ITile tile)
        {
            var (tiles, x, y, grid, size) = Setup(tile);

            for (int i = y + 1; i < size; i++)
            {
                if (grid[x, i] == null)
                {
                    tiles.Add((x, i));
                }
                else
                {
                    break; 
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                if (grid[x, i] == null)
                {
                    tiles.Add((x, i));
                }
                else
                {
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (grid[i, y] == null)
                {
                    tiles.Add((i, y));
                }
                else
                {
                    break;
                }
            }
            for (int i = x + 1; i < size; i++)
            {
                if (grid[i, y] == null)
                {
                    tiles.Add((i, y));
                }
                else
                {
                    break; 
                }
            }

            return tiles.ToList();
        }


        private List<(int row, int col)> BishopMovementOver(ITile tile)
        {
            var (tiles, x, y, grid, size) = Setup(tile);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Math.Abs(i - x) == Math.Abs(j - y))
                    {
                        if (i == x || j == y)
                        {
                            continue;
                        }
                        if (grid[i, j] == null)
                        {
                            tiles.Add((i, j));
                        }
                    }
                }
            }
            return tiles.ToList();
        }

        private List<(int row, int col)> BishopMovement(ITile tile)
        {
            HashSet<(int row, int col)> tiles = new HashSet<(int row, int col)>();
            int x = tile.GridX;
            int y = tile.GridY;
            IPawn[,] grid = m_grid.Grid;
            int size = grid.GetLength(0);

            for (int i = 1; x - i >= 0 && y - i >= 0; i++)
            {
                if (grid[x - i, y - i] == null)
                {
                    tiles.Add((x - i, y - i));
                }
                else
                {
                    break; 
                }
            }

            for (int i = 1; x - i >= 0 && y + i < size; i++)
            {
                if (grid[x - i, y + i] == null)
                {
                    tiles.Add((x - i, y + i));
                }
                else
                {
                    break;
                }
            }

            for (int i = 1; x + i < size && y - i >= 0; i++)
            {
                if (grid[x + i, y - i] == null)
                {
                    tiles.Add((x + i, y - i));
                }
                else
                {
                    break;
                }
            }

            for (int i = 1; x + i < size && y + i < size; i++)
            {
                if (grid[x + i, y + i] == null)
                {
                    tiles.Add((x + i, y + i));
                }
                else
                {
                    break;
                }
            }

            return tiles.ToList();
        }



        private List<(int row, int col)> HorseMovement(ITile tile) 
        {
            var (tiles, x, y, grid, size) = Setup(tile);
            var possibleMoves = new List<(int dx, int dy)>
            {
                (2, 1), (2, -1),
                (-2, 1), (-2, -1),
                (1, 2), (1, -2),
                (-1, 2), (-1, -2)
            };
            foreach (var (dx, dy) in possibleMoves)
            {
                int tempX = x + dx;
                int tempY = y + dy;

                if (tempX >= 0 && tempX < size && tempY >= 0 && tempY < size)
                {
                    if (grid[tempX, tempY] == null)
                    {
                        tiles.Add((tempX, tempY));
                    }
                }
            }
            return tiles.ToList();
        }

        public bool HasPotentialMove(List<IPawn> pawns, Turn turn)
        {
            return pawns
                    .Where(p => p.PawnTurn.Equals(turn))
                    .Any(pawn => CheckForAllowedMoves(pawn).Count > 0);
        }
    }
}

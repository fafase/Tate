using System.Collections.Generic;
using System.Linq;
using Tatedrez.Core;
using UnityEngine;
using Zenject;

namespace Tatedrez.Core
{
    public class PawnMovementService : MonoBehaviour, IMovementService
    {
        [Inject] private ICoreController m_core;
        private ITile[,] m_tiles;
        private PawnMovement m_movement;
        private GridController m_grid;

        void Start()
        {
            PopulateTileArray();
            m_grid = new GridController(m_core);
            m_movement = new PawnMovement(m_core, m_grid);
        }

        public void SetPotentialTiles(IPawn pawn)
        {
            List<(int row, int col)> results = m_movement.CheckForAllowedMoves(pawn);
            if(results.Count == 0) 
            {
                return;
            }
            foreach(var pos in results) 
            {
                m_tiles[pos.row, pos.col].SetTileBackground();
            }
        }

        public bool HasPotentialMove(List<IPawn> pawns, Turn nextTurn) => m_movement.HasPotentialMove(pawns, nextTurn);

        private void PopulateTileArray()
        {
            var tiles = GetComponentsInChildren<ITile>(true).ToList();
            int size = (int)Mathf.Sqrt(tiles.Count);
            int maxRow = size;
            int maxCol = size;

            m_tiles = new ITile[size, size];
            try
            {
                foreach (var tile in tiles)
                {
                    var nameParts = tile.Name.Split('_');
                    int row = int.Parse(nameParts[0]);
                    int col = int.Parse(nameParts[1]);

                    m_tiles[row, col] = tile;
                }
            }
            catch 
            {
                Debug.LogError("Something wrong with the tiles, cannot populate the array properly, check that width and length are equal");
            }
        }
    }
}
public interface IMovementService
{
    bool HasPotentialMove(List<IPawn> m_pawns, Turn nextTurn);
    void SetPotentialTiles(IPawn pawn);
}


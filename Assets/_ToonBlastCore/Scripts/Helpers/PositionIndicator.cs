using System;
using _ToonBlastCore.Scripts.Mechanic;
using Level;
using UnityEngine;

namespace Helpers
{
    public class PositionIndicator : MonoBehaviour
    {
        public int x;
        public int y;
        private void OnTriggerStay2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Tile tile)) return;

            tile.x = x;
            tile.y = y;
            FixZCoordinates(tile);
            UpdateTilesArray(tile);
        }

        void FixZCoordinates(Tile tile)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, (1 + 0.2f * y));
        }

        void UpdateTilesArray(Tile tile)
        {
            TileController.Instance.currentTiles[x][y] = tile;
        }
    }
}

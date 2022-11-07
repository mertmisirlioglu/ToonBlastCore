using _ToonBlastCore.Scripts.Managers;
using _ToonBlastCore.Scripts.Mechanic;
using Level;
using UnityEngine;

namespace Helpers
{
    public class PositionIndicator : MonoBehaviour
    {
        public int x;
        public int y;


        void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);

            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.TryGetComponent(out Tile tile)) return;

                tile.x = x;
                tile.y = y;
                FixZCoordinates(tile);
                UpdateTilesArray(tile);

                if (tile.tileType == TileTypes.Duck && y == LevelManager.Instance.levelList[LevelManager.Instance.currentLevel].tileArray.GridSize.y - 1
                                                    && !tile.checkedToDestroy)
                {
                    tile.checkedToDestroy = true;
                    tile.DestroyWithDelay();
                }
            }
        }



        void FixZCoordinates(Tile tile)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, (0.01f * y));
        }

        void UpdateTilesArray(Tile tile)
        {
            TileController.Instance.currentTiles[x][y] = tile;
        }
    }
}

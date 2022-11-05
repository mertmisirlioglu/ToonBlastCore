using System.Collections;
using System.Collections.Generic;
using _ToonBlastCore.Scripts.Managers;
using Helpers;
using Level;
using UnityEngine;

namespace _ToonBlastCore.Scripts.Mechanic
{
    public class TileController : Singleton<TileController>
    {
        public Tile[][] currentTiles;
        public List<Tile> destroyList;

        public void CheckHit(int x, int y)
        {
            destroyList = new List<Tile>();
            CheckDirection(x,y,1,0);
            CheckDirection(x,y,-1,0);
            CheckDirection(x,y,0,1);
            CheckDirection(x,y,0,-1);
        }

        IEnumerator WaitForDestroy(int x, int y)
        {
            yield return new WaitForSeconds(0.01f);
            if (currentTiles[x][y].checkedToDestroy)
            {
                Destroy(currentTiles[x][y].gameObject);
            }
        }


        void CheckDirection(int xStart, int yStart, int xDir, int yDir)
        {
            if (xStart + xDir >= currentTiles.Length ||
                xStart + xDir < 0 ||
                yStart + yDir >= currentTiles[xStart].Length ||
                yStart + yDir < 0 ||
                currentTiles[xStart + xDir][yStart+yDir] == null ||
                currentTiles[xStart + xDir][yStart+yDir].checkedToDestroy)
            {
                return;
            }

            if (currentTiles[xStart + xDir][yStart+yDir].tileType == currentTiles[xStart][yStart].tileType)
            {
                currentTiles[xStart + xDir][yStart + yDir].checkedToDestroy = true;
                currentTiles[xStart][yStart].checkedToDestroy = true;

                if (currentTiles[xStart + xDir][yStart + yDir] != null)
                {
                    EventManager.TriggerEvent("onHit", new Dictionary<string, object> { { "x", currentTiles[xStart + xDir][yStart+yDir].transform.position.x } });
                    Destroy(currentTiles[xStart + xDir][yStart + yDir]);
                }
                if (currentTiles[xStart][yStart] != null)
                {
                    EventManager.TriggerEvent("onHit", new Dictionary<string, object> { { "x", currentTiles[xStart][yStart].transform.position.x } });
                    Destroy(currentTiles[xStart][yStart]);

                }




                CheckHit(xStart +xDir, yStart + yDir);
            }



        }

    }
}

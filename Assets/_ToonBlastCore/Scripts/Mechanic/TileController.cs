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
                currentTiles[xStart + xDir][yStart + yDir].GetComponent<SpriteRenderer>().color = Color.black;
                currentTiles[xStart][yStart].checkedToDestroy = true;
                destroyList.Add(currentTiles[xStart + xDir][yStart + yDir]);
                destroyList.Add(currentTiles[xStart][yStart]);
                Debug.Log("eşlendim");
                // currentTiles[xStart + xDir][yStart+yDir].gameObject.SetActive(false);
                Destroy(currentTiles[xStart + xDir][yStart+yDir].gameObject);
                Destroy(currentTiles[xStart][yStart].gameObject);
                EventManager.TriggerEvent();
                CheckHit(xStart +xDir, yStart + yDir);
            }



        }

    }
}

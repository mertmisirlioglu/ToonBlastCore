using System;
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

        private void Start()
        {
            EventManager.StartListening("onTileClicked", OnTileClicked);
        }

        private void OnTileClicked(Dictionary<string, object> message)
        {
            CheckHit((TileTypes)message["tileType"],(int) message["x"], (int) message["y"]);
        }

        public void CheckHit(TileTypes tileType , int x, int y)
        {
            destroyList = new List<Tile>();
            int hitCount = CheckNeighbours(tileType,x,y);
            Debug.Log("hitcount is : " + hitCount);

            if (hitCount == 1)
            {
                // baloons are not counted as hit count
                foreach (var tile in destroyList)
                {
                    tile.checkedToDestroy = false;
                }
                return;
            }

            if(hitCount < 1)
            {
                return;
            }

            if (hitCount >= 5)
            {
                EventManager.TriggerEvent("CreateRocket", new Dictionary<string, object> { {"x" ,currentTiles[x][y].transform.position.x } , {"y" , currentTiles[x][y].transform.position.y}});
                destroyList[0].createNewTile = false;
            }

            EventManager.TriggerEvent("onMove", null);



            foreach (var tile in destroyList)
            {
                tile.DestroyWithDelay();
            }
        }

        int CheckNeighbours(TileTypes type, int x, int y)
        {
            if (x >= currentTiles.Length ||
                x < 0 ||
                y >= currentTiles[x].Length ||
                y < 0 ||
                currentTiles[x][y] == null ||
                currentTiles[x][y].checkedToDestroy)
            {
                return 0;
            }

            if (currentTiles[x][y].notMatchable)
            {
                if (currentTiles[x][y].tileType == TileTypes.Balloon)
                {
                    destroyList.Add(currentTiles[x][y]);
                    currentTiles[x][y].checkedToDestroy = true;
                }
                return 0;
            }

            if (currentTiles[x][y].tileType == type)
            {
                currentTiles[x][y].checkedToDestroy = true;
                destroyList.Add(currentTiles[x][y]);

                return CheckNeighbours(type, x + 1, y) + CheckNeighbours(type, x - 1, y) +
                       CheckNeighbours(type, x, y + 1) + CheckNeighbours(type, x, y - 1) + 1;
            }

            return 0;


        }

    }
}

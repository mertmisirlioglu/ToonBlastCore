using System;
using _ToonBlastCore.Scripts.Managers;
using _ToonBlastCore.Scripts.Mechanic;
using UnityEngine;

namespace Level
{
    public class Tile: MonoBehaviour
    {
        public TileTypes tileType;
        public int x;
        public int y;
        public bool checkedToDestroy;

        private float timer = 0;



        void OnMouseDown ()
        {
            if (timer > Time.time * 1000 || GameManager.gameState != GameState.Playing)
            {
                return;
            }
            Debug.Log("çalıstım");

            timer = Time.time * 1000 + 500;
            TileController.Instance.CheckHit(tileType,x, y);
        }

    }
}

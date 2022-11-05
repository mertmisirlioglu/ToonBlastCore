using System;
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
            if (timer > Time.time * 1000)
            {
                return;
            }
            Debug.Log("çalıstım");

            timer = Time.time * 1000 + 500;
            TileController.Instance.CheckHit(x, y);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (tileType == TileTypes.Duck)
            {
                Debug.Log("collision name is" + collision.gameObject.name);
            }
        }
    }
}

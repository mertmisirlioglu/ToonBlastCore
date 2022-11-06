using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using _ToonBlastCore.Scripts.Managers;
using _ToonBlastCore.Scripts.Mechanic;
using Unity.VisualScripting;
using UnityEngine;

namespace Level
{
    public class Tile: MonoBehaviour
    {
        public TileTypes tileType;
        public int x;
        public int y;
        public bool checkedToDestroy;
        [SerializeField] private ParticleSystem PFX_destroy;

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

        public void DestroyWithDelay()
        {
            Instantiate(PFX_destroy, transform.position, transform.rotation);
            StartCoroutine(DelayedDestroy());
        }

        IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(0.2f);
            EventManager.TriggerEvent("onHit", new Dictionary<string, object> { { "x", transform.position.x } });
            Destroy(gameObject);
        }


    }
}

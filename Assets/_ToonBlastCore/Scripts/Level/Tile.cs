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
        public bool notMatchable;
        public bool createNewTile = true;
        [SerializeField] private ParticleSystem PFX_destroy;

        private float timer = 0;

        private BoxCollider2D boxCollider;
        private Rigidbody2D rb;

        private void Start()
        {
            if (tileType == TileTypes.Balloon || tileType == TileTypes.Duck)
            {
                notMatchable = true;
            }

            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }


        void OnMouseDown ()
        {
            if (timer > Time.time * 1000 || GameManager.gameState != GameState.Playing)
            {
                return;
            }
            EventManager.TriggerEvent("onTileClicked", new Dictionary<string, object> {{ "tileType", tileType } , {"x" , x} , {"y" , y}});
            timer = Time.time * 1000 + 500;
        }

        public void DestroyWithDelay()
        {
            if (tileType != TileTypes.Duck && tileType != TileTypes.Balloon)
            {
                Instantiate(PFX_destroy, transform.position, transform.rotation);
            }
            StartCoroutine(DelayedDestroy());
        }

        IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(0.2f);
            if(createNewTile) EventManager.TriggerEvent("onHit", new Dictionary<string, object> { { "x", transform.position.x } });
            var level = LevelManager.Instance.levelList[LevelManager.Instance.currentLevel];

            if (level.firstGoalTile == tileType || level.secondGoalTile == tileType)
            {
                EventManager.TriggerEvent("onGoalHit", new Dictionary<string, object> { { "tileType", tileType} });

                StartCoroutine(GoToGoalAnimation(level.firstGoalTile == tileType));
            }
            else
            {
                Destroy(gameObject);
            }


        }

        IEnumerator GoToGoalAnimation(bool isFirstGoal)
        {
            boxCollider.enabled = false;
            rb.isKinematic = true;
            var target = isFirstGoal ? new Vector3(-0.06f,4.21f,0f) : new Vector3(0.67f,4.3f,0f);

            while (Vector3.Distance(transform.position,target) > 0.02f)
            {
                transform.position = Vector3.Lerp(transform.position, target, 0.1f );
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.01f);
                yield return new WaitForFixedUpdate();
            }

            Destroy(gameObject);
            yield return null;
        }


    }
}

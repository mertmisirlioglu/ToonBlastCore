using System;
using System.Collections;
using System.Collections.Generic;
using _ToonBlastCore.Scripts.Mechanic;
using Helpers;
using Level;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _ToonBlastCore.Scripts.Managers
{
    public class GridManager : Helpers.Singleton<GridManager>
    {
        [SerializeField]
        private Tile[] baseTileList;
        public Dictionary<TileTypes, Tile> enumToTilesDictionary;

        [SerializeField] private Transform tileContainer;
        [SerializeField] private Transform gameContainer;
        [SerializeField] private Transform bottomLeftTransform;
        [SerializeField] private Transform bottomRightTransform;
        [SerializeField] private Transform topLeftTransform;
        [SerializeField] private SpriteRenderer gameAreaBorderSprite;

        public Transform positionIndicatorContainer;
        public Transform indicatorPrefab;

        public GameObject rocketGameObject;

        private Tile[][] currentTiles;



        private void Start()
        {
            EventManager.StartListening("onLevelLoaded", OnLevelLoaded);
            EventManager.StartListening("onHit", OnHit);
            EventManager.StartListening("CreateRocket", CreateRocket);

            enumToTilesDictionary = Utils.CreateEnumToTilesDictionary(baseTileList);
        }

        private void OnLevelLoaded(Dictionary<string, object> message)
        {
            CreateTiles();
        }

        private void OnHit(Dictionary<string, object> message)
        {
            StartCoroutine(InstantiateTileOnSky((float)message["x"]));
        }

        private void CreateTiles()
        {
            var tileArray = LevelManager.Instance.currentLevelObject.tileArray;
            currentTiles = new Tile[tileArray.GridSize.x][];

            var currentX = bottomLeftTransform.position.x;
            var totalX = bottomRightTransform.position.x - bottomLeftTransform.position.x;
            var stepX = totalX / 8 + 0.018f; // Calculations are made for 9x9 grid

            var currentY = bottomLeftTransform.position.y;
            var totalY = topLeftTransform.position.y - bottomLeftTransform.position.y;
            var stepY = totalY / 8;

            for (int i = 0; i < tileArray.GridSize.x; i++)
            {
                currentY = bottomLeftTransform.position.y;
                for (int j = tileArray.GridSize.y - 1; j >= 0; j--)
                {
                    InstantiateTile(i,j, new Vector3(currentX, currentY,0), tileContainer);
                    currentY += stepY;
                }
                currentX += stepX;
            }

            UpdateContainers(stepX,stepY);
            TileController.Instance.currentTiles = currentTiles;
            GameManager.gameState = GameState.Playing;
            UIManager.Instance.SetGoalsAndMoveCount();
        }

        private void InstantiateTile(int x, int y, Vector3 pos , Transform container)
        {
            LevelScriptableObject level = LevelManager.Instance.currentLevelObject;
            var tileType = level.tileArray.GetCell(x, y);

            currentTiles[x] ??= new Tile[level.tileArray.GridSize.y];

            currentTiles[x][y] = Instantiate(enumToTilesDictionary[tileType].gameObject, pos, quaternion.identity,container).GetComponent<Tile>();
            currentTiles[x][y].x = x;
            currentTiles[x][y].y = y;

            // pos.z = 5;
            pos.y -= 0.06f;
            var indicator = Instantiate(indicatorPrefab, pos, quaternion.identity, positionIndicatorContainer);
            indicator.GetComponent<PositionIndicator>().x = x;
            indicator.GetComponent<PositionIndicator>().y = y;
        }

        private IEnumerator InstantiateTileOnSky(float x)
        {
            GameManager.gameState = GameState.Stop;
            var pos = new Vector3(x, 5, 0);
            TileTypes tileType = Utils.RandomEnumValue<TileTypes>();
            yield return new WaitForSeconds(Random.Range(0, 0.17f));
            Instantiate(enumToTilesDictionary[tileType].gameObject, pos, quaternion.identity,tileContainer).GetComponent<Tile>();
            GameManager.gameState = GameState.Playing;
        }



        private void UpdateContainers(float stepX, float stepY)
        {
            var tileArray = LevelManager.Instance.currentLevelObject.tileArray;

            if (tileArray.GridSize.x == 9 && tileArray.GridSize.y == 9)
            {
                return;
            }

            var position = tileContainer.position;
            position = new Vector3(position.x + (9f - tileArray.GridSize.x) / 2 * stepX,
                position.y, 0);
            tileContainer.position = position;

            var position1 = gameContainer.position;
            position1 = new Vector3(position1.x,
                position1.y + (9f - tileArray.GridSize.y) / 2 * stepY, 0);
            gameContainer.position = position1;

            var position2 = positionIndicatorContainer.position;
            position2 = new Vector3(position2.x + (9f - tileArray.GridSize.x) / 2 * stepX,
                position2.y + (9f - tileArray.GridSize.y) / 2 * stepY,0);
            positionIndicatorContainer.position = position2;

            var size = gameAreaBorderSprite.size;
            size = new Vector2(size.x -
                size.x / 9f * (9f - tileArray.GridSize.x) + 0.15f, size.y -
                size.y / 9f * (9f - tileArray.GridSize.y) + 0.15f);
            gameAreaBorderSprite.size = size;
        }


        private void CreateRocket(Dictionary<string, object> message)
        {
            Instantiate(rocketGameObject, new Vector3((float) message["x"], (float) message["y"], 0), quaternion.identity,tileContainer);
        }

    }
}

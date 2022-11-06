using System;
using System.Collections;
using System.Collections.Generic;
using _ToonBlastCore.Scripts.Mechanic;
using Helpers;
using Level;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _ToonBlastCore.Scripts.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelScriptableObject[] levelList;

        [SerializeField]
        private Tile[] baseTileList;


        [SerializeField] private Transform tileContainer;
        [SerializeField] private Transform gameContainer;
        [SerializeField] private Transform bottomLeftTransform;
        [SerializeField] private Transform bottomRightTransform;
        [SerializeField] private Transform topLeftTransform;
        [SerializeField] private Transform topRightTransform;
        [SerializeField] private SpriteRenderer gameAreaBorderSprite;

        public Transform positionIndicatorContainer;
        public Transform indicatorPrefab;

        public Dictionary<TileTypes, Tile> enumToTilesDictionary;
        private Tile[][] currentTiles;

        public int currentLevel;
        public int remainingMoves;

        public TileTypes firstGoalType;
        public int firstGoalValue;
        public TileTypes secondGoalType;
        public int secondGoalValue;


        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("onTileClicked", OnTileClicked);
            EventManager.StartListening("onHit", OnHit);
            CreateEnumToTilesDictionary();
        }

        private void OnGameStart(Dictionary<string, object> message)
        {
            GetLevelData();
            CreateTiles();
        }

        private void GetLevelData()
        {
            remainingMoves = levelList[currentLevel].totalMove;
            firstGoalType = levelList[currentLevel].firstGoalTile;
            firstGoalValue = levelList[currentLevel].firstGoalValue;
            secondGoalType = levelList[currentLevel].secondGoalTile;
            secondGoalValue = levelList[currentLevel].secondGoalValue;
        }

        private void OnHit(Dictionary<string, object> message)
        {
            StartCoroutine(InstantiateTileOnSky((float)message["x"]));
        }

        private void CreateEnumToTilesDictionary()
        {
            if (baseTileList == null)
            {
                throw new UnityException("Tile list is empty!");
            }

            enumToTilesDictionary = new Dictionary<TileTypes, Tile>();

            for (int i = 0; i < baseTileList.Length; i++)
            {
                enumToTilesDictionary.Add(baseTileList[i].tileType, baseTileList[i]);
            }
        }

        private void CreateTiles()
        {
            if (levelList[currentLevel] == null)
            {
                throw new UnityException("Level data is empty! Check level scriptable object");
            }

            var tileArray = levelList[currentLevel].tileArray;
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
        }

        private void InstantiateTile(int x, int y, Vector3 pos , Transform container)
        {
            var tileType = levelList[currentLevel].tileArray.GetCell(x, y);

            currentTiles[x] ??= new Tile[levelList[currentLevel].tileArray.GridSize.y];

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
            TileTypes tileType = RandomEnumValue<TileTypes>();
            yield return new WaitForSeconds(Random.Range(0, 0.17f));
            Instantiate(enumToTilesDictionary[tileType].gameObject, pos, quaternion.identity,tileContainer).GetComponent<Tile>();
            GameManager.gameState = GameState.Playing;
        }

        public static T RandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));
            int random = Random.Range(0, values.Length);
            return (T)values.GetValue(random);
        }

        private void UpdateContainers(float stepX, float stepY)
        {
            var tileArray = levelList[currentLevel].tileArray;

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

        private void OnTileClicked(Dictionary<string, object> message)
        {
            if (remainingMoves - 1 < 0)
            {
                EventManager.TriggerEvent("onLose", null);
                return;
            }

            remainingMoves--;
            EventManager.TriggerEvent("UpdateUI", null);
            EventManager.TriggerEvent("eligibleToMove", message);

        }



        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
        }

    }
}

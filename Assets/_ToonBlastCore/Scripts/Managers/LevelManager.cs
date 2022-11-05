using System.Collections.Generic;
using _ToonBlastCore.Scripts.Mechanic;
using Level;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace _ToonBlastCore.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private LevelScriptableObject[] levelList;

        [SerializeField]
        private Tile[] baseTileList;


        [SerializeField] private Transform tileContainer;
        [SerializeField] private Transform gameContainer;
        [SerializeField] private Transform bottomLeftTransform;
        [SerializeField] private Transform bottomRightTransform;
        [SerializeField] private Transform topLeftTransform;
        [SerializeField] private Transform topRightTransform;
        [SerializeField] private SpriteRenderer gameAreaBorderSprite;

        private Dictionary<TileTypes, Tile> enumToTilesDictionary;
        private Tile[][] currentTiles;

        public int currentLevel;

        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            CreateEnumToTilesDictionary();
        }

        private void OnGameStart(Dictionary<string, object> message)
        {
            CreateTiles();
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
                float currentZ = 0;
                for (int j = tileArray.GridSize.y - 1; j >= 0; j--)
                {
                    InstantiateTile(i,j, new Vector3(currentX, currentY,currentZ), tileContainer);
                    currentY += stepY;
                    currentZ -= 0.01f;
                }
                currentX += stepX;
            }

            UpdateContainers(stepX,stepY);
            TileController.Instance.currentTiles = currentTiles;
        }

        private void InstantiateTile(int x, int y, Vector3 pos , Transform container)
        {
            var tileType = levelList[currentLevel].tileArray.GetCell(x, y);

            currentTiles[x] ??= new Tile[levelList[currentLevel].tileArray.GridSize.y];

            currentTiles[x][y] = Instantiate(enumToTilesDictionary[tileType].gameObject, pos, quaternion.identity,container).GetComponent<Tile>();
            currentTiles[x][y].x = x;
            currentTiles[x][y].y = y;
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

            var size = gameAreaBorderSprite.size;
            size = new Vector2(size.x -
                size.x / 9f * (9f - tileArray.GridSize.x) + 0.15f, size.y -
                size.y / 9f * (9f - tileArray.GridSize.y) + 0.15f);
            gameAreaBorderSprite.size = size;
        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
        }

    }
}

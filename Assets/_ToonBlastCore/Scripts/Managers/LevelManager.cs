using System.Collections.Generic;
using Level;
using Unity.Mathematics;
using UnityEngine;

namespace _ToonBlastCore.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private LevelScriptableObject[] levelList;

        [SerializeField]
        private Tile[] baseTileList;


        [SerializeField] private Transform tileContainer;
        [SerializeField] private Transform bottomLeftTransform;
        [SerializeField] private Transform bottomRightTransform;
        [SerializeField] private Transform topLeftTransform;
        [SerializeField] private Transform topRightTransform;

        private Dictionary<TileTypes, Tile> enumToTilesDictionary;

        public int currentLevel;

        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            CreateEnumToTilesDictionary();
        }

        private void OnGameStart(Dictionary<string, object> message)
        {
            Debug.Log("game started eheehe");
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

            var currentX = bottomLeftTransform.position.x;
            var totalX = bottomRightTransform.position.x - bottomLeftTransform.position.x;
            var step = totalX / 8; // Calculations are made for 9x9 grid

            var currentY = bottomLeftTransform.position.y;
            var totalY = topLeftTransform.position.y - bottomLeftTransform.position.y;
            var stepY = totalY / 8;

            var currentZ = 0f;

            for (int i = 0; i < tileArray.GridSize.x; i++)
            {
                currentY = bottomLeftTransform.position.y;
                currentZ = 0;
                for (int j = tileArray.GridSize.y - 1; j >= 0; j--)
                {
                    InstantiateTile(tileArray.GetCell(i, j), new Vector3(currentX, currentY,currentZ), tileContainer);
                    currentY += stepY;
                    currentZ -= 0.01f;

                    // Debug.Log("hey bunun adı :" + );
                }
                currentX += step;
            }

            // Physics2D.gravity = Vector2.zero;
        }

        private void InstantiateTile(TileTypes tileType, Vector3 pos , Transform container)
        {
            Instantiate(enumToTilesDictionary[tileType].gameObject, pos, quaternion.identity,container);
        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
        }

    }
}

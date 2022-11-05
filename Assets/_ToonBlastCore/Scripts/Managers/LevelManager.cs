using System.Collections.Generic;
using Level;
using UnityEngine;

namespace _ToonBlastCore.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private LevelScriptableObject[] levelList;

        [SerializeField]
        private Tile[] tileList;

        [SerializeField] private Transform tileContainer;
        [SerializeField] private Transform bottomLeftTransform;
        [SerializeField] private Transform bottomRightTransform;

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
            if (tileList == null)
            {
                throw new UnityException("Tile list is empty!");
            }

            enumToTilesDictionary = new Dictionary<TileTypes, Tile>();

            for (int i = 0; i < tileList.Length; i++)
            {
                enumToTilesDictionary.Add(tileList[i].tileType, tileList[i]);
            }
        }

        private void CreateTiles()
        {
            if (levelList[currentLevel] == null)
            {
                throw new UnityException("Level data is empty! Check level scriptable object");
            }

            var tileArray = levelList[currentLevel].tileArray;

            for (int i = 0; i < tileArray.GridSize.x; i++)
            {
                for (int j = tileArray.GridSize.y - 1; j >= 0; j--)
                {
                    Debug.Log("hey bunun adı :" + tileArray.GetCell(i,j));
                }
            }


        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
        }

    }
}

using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
    public class LevelScriptableObject : ScriptableObject
    {

        [Header("Goal Settings")]
        public TileTypes firstGoalTile;
        public int firstGoalValue;

        public TileTypes secondGoalTile;
        public int secondGoalValue;

        [Header("Move count")] public int totalMove;

        [Space(10)]
        [SerializeField]
        public Array2DTilesEnum tileArray;

    }
}

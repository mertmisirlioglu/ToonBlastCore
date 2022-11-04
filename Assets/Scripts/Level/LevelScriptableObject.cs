using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
    public class LevelScriptableObject : ScriptableObject
    {
        [SerializeField]
        private Array2DTilesEnum arrayEnum;
    }
}

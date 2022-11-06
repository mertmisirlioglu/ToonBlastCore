using System.Collections.Generic;
using Level;
using Unity.VisualScripting;
using UnityEngine;

namespace _ToonBlastCore.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("onGameEnd", OnGameEnd);
            EventManager.StartListening("onScoreChange", OnScoreChange);
            EventManager.StartListening("onGoalHit", OnGoalHit);
        }

        private void OnGameStart(Dictionary<string, object> message)
        {

        }

        private void OnGameEnd(Dictionary<string, object> message)
        {

        }
        private void OnScoreChange(Dictionary<string, object> message)
        {

        }

        private void OnGoalHit(Dictionary<string, object> message)
        {
            var level = LevelManager.Instance;

            if (level.firstGoalType == (TileTypes)message["tileType"] && level.firstGoalValue > 0)
            {
                level.firstGoalValue--;
            }
            else if (level.secondGoalValue > 0)
            {
                level.secondGoalValue--;
            }


            EventManager.TriggerEvent("UpdateUI", null);
        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
            EventManager.StopListening("onGameEnd", OnGameEnd);
            EventManager.StopListening("onScoreChange", OnScoreChange);
        }


    }
}

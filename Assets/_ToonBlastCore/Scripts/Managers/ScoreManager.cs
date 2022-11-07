using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Level;
using Unity.VisualScripting;
using UnityEngine;

namespace _ToonBlastCore.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private bool isFirstGoalDone;
        private bool isSecondGoalDone;

        private void Start()
        {
            EventManager.StartListening("onGameEnd", OnGameEnd);
            EventManager.StartListening("onScoreChange", OnScoreChange);
            EventManager.StartListening("onGoalHit", OnGoalHit);
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

            if (level.firstGoalType == (TileTypes)message["tileType"] && !isFirstGoalDone)
            {
                level.firstGoalValue--;
            }
            else if (level.secondGoalType == (TileTypes)message["tileType"] && !isSecondGoalDone)
            {
                level.secondGoalValue--;
            }

            if (!isFirstGoalDone && level.firstGoalValue == 0)
            {
                isFirstGoalDone = true;
                EventManager.TriggerEvent("onGoalDone", new Dictionary<string, object>{{"isFirstGoal", true}});
            }

            if (!isSecondGoalDone && level.secondGoalValue == 0)
            {
                isSecondGoalDone = true;
                EventManager.TriggerEvent("onGoalDone", new Dictionary<string, object>{{"isFirstGoal", false}});
            }

            if (isFirstGoalDone && isSecondGoalDone)
            {
                StartCoroutine(Utils.DelayedAction(() => EventManager.TriggerEvent("onWin", null), 2.5f));
            }


            EventManager.TriggerEvent("UpdateUI", null);
        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameEnd", OnGameEnd);
            EventManager.StopListening("onScoreChange", OnScoreChange);
        }




    }
}

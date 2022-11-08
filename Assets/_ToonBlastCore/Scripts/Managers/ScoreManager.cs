using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Level;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _ToonBlastCore.Scripts.Managers
{
    public class ScoreManager : Helpers.Singleton<ScoreManager>
    {
        private bool isFirstGoalDone;
        private bool isSecondGoalDone;

        public TileTypes firstGoalType;
        public int firstGoalValue;
        public TileTypes secondGoalType;
        public int secondGoalValue;

        private void Start()
        {
            EventManager.StartListening("onLevelLoaded", OnLevelLoaded);
            EventManager.StartListening("onGoalHit", OnGoalHit);
        }

        private void OnLevelLoaded(Dictionary<string, object> message)
        {
            LevelScriptableObject level = LevelManager.Instance.currentLevelObject;
            firstGoalType = level.firstGoalTile;
            firstGoalValue = level.firstGoalValue;
            secondGoalType = level.secondGoalTile;
            secondGoalValue = level.secondGoalValue;
        }


        private void OnGoalHit(Dictionary<string, object> message)
        {
            if (firstGoalType == (TileTypes)message["tileType"] && !isFirstGoalDone)
            {
                firstGoalValue--;
            }
            else if (secondGoalType == (TileTypes)message["tileType"] && !isSecondGoalDone)
            {
                secondGoalValue--;
            }

            if (!isFirstGoalDone && firstGoalValue == 0)
            {
                isFirstGoalDone = true;
                EventManager.TriggerEvent("onGoalDone", new Dictionary<string, object>{{"isFirstGoal", true}});
            }

            if (!isSecondGoalDone && secondGoalValue == 0)
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
            EventManager.StopListening("onLevelLoaded", OnLevelLoaded);
            EventManager.StopListening("onGoalHit", OnGoalHit);
        }




    }
}

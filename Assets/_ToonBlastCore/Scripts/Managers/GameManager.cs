using System;
using System.Collections.Generic;
using _ToonBlastCore.Scripts.Managers;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _ToonBlastCore.Scripts.Managers
{
    public enum GameState
    {
        Playing,
        Stop
    }
    public class GameManager : Singleton<GameManager>
    {
        public static GameState gameState;

        private Coroutine loseCoroutine;
        public int remainingMoves;

        private void Start()
        {
            EventManager.StartListening("onLevelLoaded", OnLevelLoaded);
            EventManager.StartListening("onMove", OnMove);
            EventManager.StartListening("onWin", OnWin);

        }

        private void OnLevelLoaded(Dictionary<string, object> message)
        {
            remainingMoves = LevelManager.Instance.currentLevelObject.totalMove;
        }


        // for last move wins
        private void OnWin(Dictionary<string, object> message)
        {
            if (loseCoroutine != null)
            {
                StopCoroutine(loseCoroutine);
            }
        }

        private void OnMove(Dictionary<string, object> message)
        {
            remainingMoves--;

            if (remainingMoves <= 0)
            {
                loseCoroutine = StartCoroutine(Utils.DelayedAction(() => EventManager.TriggerEvent("onLose", null),5f));
                return;
            }

            EventManager.TriggerEvent("UpdateUI", null);
        }



    }
}
